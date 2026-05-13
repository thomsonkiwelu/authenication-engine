using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.HabitatManipulations.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.HabitatManipulations;

public class HabitatManipulationRepository(AppDBContext context, IUserContext userContext): IHabitatManipulationRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<HabitatManipulationResponseDto>> GetPagedData(HabitatManipulationPaginationDto dto)
    {
        var connectionString = _context.Database.GetConnectionString();
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_habitat_manipulations(@page, @pageSize, @search, @actionTaken, @parkId, @parkIds)",
            connection
        );

        // Add parameters with proper null handling
        command.Parameters.AddWithValue("page", dto.page);
        command.Parameters.AddWithValue("pageSize", dto.pageSize);
        command.Parameters.AddWithValue("search", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.q) ? (object)DBNull.Value : dto.q);
        command.Parameters.AddWithValue("actionTaken", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.ActionTaken) ? (object)DBNull.Value : dto.ActionTaken);
        command.Parameters.AddWithValue("parkId", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.ParkId) ? (object)DBNull.Value : dto.ParkId);
        // Add park IDs parameter
        command.Parameters.AddWithValue("parkIds", NpgsqlDbType.Array | NpgsqlDbType.Uuid,
            parkIds.Any() ? parkIds.ToArray() : (object)DBNull.Value);

        var jsonResult = await command.ExecuteScalarAsync() as string;
        if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get habitat manipulation data");
    
        var apiResponse = JsonSerializer.Deserialize<HabitatManipulationSqlResponseDto>(
            jsonResult,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            }
        );

        return new PagedList<HabitatManipulationResponseDto>(
            apiResponse!.Data,
            dto.page,
            dto.pageSize,
            apiResponse.Meta.TotalItems
        );
    }

    public async Task<string> Create(HabitatManipulationRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_habitat_manipulation(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetHabitatManipulationDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_habitat_manipulation_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get habitat manipulation data");

        var result = JsonSerializer.Deserialize<GetHabitatManipulationDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetHabitatManipulationDto();
    }

    public async Task<string> Update(Guid id, HabitatManipulationRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_habitat_manipulation(@data::jsonb, @Id)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("Id", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var habitatManipulation = await _context.HabitatManipulations.FindAsync(id);
        if (habitatManipulation is null)
            throw new KeyNotFoundException("Habitat manipulation record not found");

        habitatManipulation.DeletedAt = DateTime.UtcNow;
        habitatManipulation.UpdatedBy = _userContext.GetUserId();
        habitatManipulation.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}