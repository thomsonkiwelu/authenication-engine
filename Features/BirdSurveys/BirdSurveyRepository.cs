using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.BirdSurveys.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.BirdSurveys;

public class BirdSurveyRepository(AppDBContext context, IUserContext userContext): IBirdSurveyRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<BirdSurveyResponseDto>> GetPagedData(BirdSurveyPaginationDto dto)
    {
        var connectionString = _context.Database.GetConnectionString();
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_bird_surveys(@page, @pageSize, @search, @parkId, @parkIds)",
            connection
        );
    
        // Add parameters with proper null handling
        command.Parameters.AddWithValue("page", dto.page);
        command.Parameters.AddWithValue("pageSize", dto.pageSize);
        command.Parameters.AddWithValue("search", NpgsqlDbType.Varchar, 
            string.IsNullOrWhiteSpace(dto.q) ? (object)DBNull.Value : dto.q);
        command.Parameters.AddWithValue("parkId", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.ParkId) ? (object)DBNull.Value : dto.ParkId);
        // Add park IDs parameter
        command.Parameters.AddWithValue("parkIds", NpgsqlDbType.Array | NpgsqlDbType.Uuid,
            parkIds.Any() ? parkIds.ToArray() : (object)DBNull.Value);
    
        var jsonResult = await command.ExecuteScalarAsync() as string;
        if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get bird survey data");
        
        var apiResponse = JsonSerializer.Deserialize<BirdSuverySqlResponseDto>(
            jsonResult,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            }
        );
    
        return new PagedList<BirdSurveyResponseDto>(
            apiResponse!.Data,
            dto.page,
            dto.pageSize,
            apiResponse.Meta.TotalItems
        );
    }

    public async Task<string> Create(BirdSurveyRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_bird_survey(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetBirdSurveyDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_bird_survey_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get bird survey data");

        var result = JsonSerializer.Deserialize<GetBirdSurveyDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetBirdSurveyDto();
    }

    public async Task<string> Update(Guid id, BirdSurveyRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_bird_survey(@data::jsonb, @birdSurveyId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("birdSurveyId", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var birdSurvey = await _context.BirdSurveys.FindAsync(id);
        if (birdSurvey is null)
            throw new KeyNotFoundException("Bird survey record not found");

        birdSurvey.DeletedAt = DateTime.UtcNow;
        birdSurvey.UpdatedBy = _userContext.GetUserId();
        birdSurvey.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}