using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.Vegetation.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.Vegetation;

public class VegetationRepository(AppDBContext context, IUserContext userContext) : IVegetationRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<VegetationResponseDto>> GetPagedData(VegetationPaginationDto dto)
    {
        var connectionString = _context.Database.GetConnectionString();
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_vegetations(@page, @pageSize, @search, @vegType, @methodType, @lifeFormType, @parkId, @parkIds)",
            connection
        );
        
        // Add parameters
        command.Parameters.AddWithValue("page", dto.page);
        command.Parameters.AddWithValue("pageSize", dto.pageSize);
        command.Parameters.AddWithValue("search", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.q) ? (object)DBNull.Value : dto.q);
        command.Parameters.AddWithValue("vegType", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.VegetationType) ? (object)DBNull.Value : dto.VegetationType);
        command.Parameters.AddWithValue("methodType", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.MethodologyType) ? (object)DBNull.Value : dto.MethodologyType);
        command.Parameters.AddWithValue("lifeFormType", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.LifeFormType) ? (object)DBNull.Value : dto.LifeFormType);
        command.Parameters.AddWithValue("parkId", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.ParkId) ? (object)DBNull.Value : dto.ParkId);
        // Add park IDs parameter
        command.Parameters.AddWithValue("parkIds", NpgsqlDbType.Array | NpgsqlDbType.Uuid,
            parkIds.Any() ? parkIds.ToArray() : (object)DBNull.Value);
    
        var jsonResult = await command.ExecuteScalarAsync() as string;
        if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get vegetation data");
        
        var apiResponse = JsonSerializer.Deserialize<ListVegetationSqlResponseDto>(
            jsonResult,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                //Converters = { new EmptyStringToNullConverter() } INFO::need to remove null property
            }
        );
        
        return new PagedList<VegetationResponseDto>(
            apiResponse!.Data,
            dto.page,
            dto.pageSize,
            apiResponse.Meta.TotalItems
        );
    }

    public async Task<string> Create(VegetationRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_vegetation(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetVegetationResponseDto> GetById(Guid id)
    {
        var dataSql = "SELECT * FROM fn_vegetations_by_id({0})";

        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get vegetation data");

        var result = JsonSerializer.Deserialize<GetVegetationResponseDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetVegetationResponseDto();
    }

    public async Task<string> Update(Guid id, VegetationRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_vegetation(@data::jsonb, @vegetationId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("vegetationId", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        Console.WriteLine(result);
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var vegetation = await _context.Vegetations.FindAsync(id);
        if (vegetation is null)
            throw new KeyNotFoundException("Vegetation record not found");

        vegetation.DeletedAt = DateTime.UtcNow;
        vegetation.UpdatedBy = _userContext.GetUserId();
        vegetation.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}