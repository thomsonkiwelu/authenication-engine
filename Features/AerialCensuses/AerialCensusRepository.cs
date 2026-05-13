using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.AerialCensuses.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.AerialCensuses;

public class AerialCensusRepository(AppDBContext context, IUserContext userContext): IAerialCensusRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<AerialCensusResponseDto>> GetPagedData(AerialCensusPaginationDto dto)
    {
        var connectionString = _context.Database.GetConnectionString();
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_aerial_censuses(@page, @pageSize, @startDate, @endDate, @parkId, @parkIds)",
            connection
        );

        // Add parameters with proper null handling
        command.Parameters.AddWithValue("page", dto.page);
        command.Parameters.AddWithValue("pageSize", dto.pageSize);
        command.Parameters.AddWithValue("startDate", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.StartDate) ? (object)DBNull.Value : dto.StartDate);
        command.Parameters.AddWithValue("endDate", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.EndDate) ? (object)DBNull.Value : dto.EndDate);
        command.Parameters.AddWithValue("parkId", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.ParkId) ? (object)DBNull.Value : dto.ParkId);
        // Add park IDs parameter
        command.Parameters.AddWithValue("parkIds", NpgsqlDbType.Array | NpgsqlDbType.Uuid,
            parkIds.Any() ? parkIds.ToArray() : (object)DBNull.Value);

        var jsonResult = await command.ExecuteScalarAsync() as string;
        if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get aerial census data");
    
        var apiResponse = JsonSerializer.Deserialize<AerialCensusSqlResponseDto>(
            jsonResult,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            }
        );

        return new PagedList<AerialCensusResponseDto>(
            apiResponse!.Data,
            dto.page,
            dto.pageSize,
            apiResponse.Meta.TotalItems
        );
    }

    public async Task<string> Create(AerialCensusRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_aerial_census(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetAerialCensusDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_aerial_census_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get aerial census data");

        var result = JsonSerializer.Deserialize<GetAerialCensusDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetAerialCensusDto();
    }

    public async Task<string> Update(Guid id, AerialCensusRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_aerial_census(@data::jsonb, @Id)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("Id", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var aerialCensus = await _context.AerialCensuses.FindAsync(id);
        if (aerialCensus is null)
            throw new KeyNotFoundException("Aerial census record not found");

        aerialCensus.DeletedAt = DateTime.UtcNow;
        aerialCensus.UpdatedBy = _userContext.GetUserId();
        aerialCensus.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}