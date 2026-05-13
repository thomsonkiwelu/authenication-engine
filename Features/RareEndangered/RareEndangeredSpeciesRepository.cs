using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.RareEndangered.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.RareEndangered;

public class RareEndangeredSpeciesRepository(AppDBContext context, IUserContext userContext): IRareEndangeredSpeciesRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<RareEndangeredSpeciesResponseDto>> GetPagedData(RareEndangeredSpeciesPaginationDto dto)
    {
        var connectionString = _context.Database.GetConnectionString();
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_rare_endangered_species(@page, @pageSize, @search, @parkId, @category, @parkIds)",
            connection
        );
    
        // Add parameters with proper null handling
        command.Parameters.AddWithValue("page", dto.page);
        command.Parameters.AddWithValue("pageSize", dto.pageSize);
        command.Parameters.AddWithValue("search", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.q) ? (object)DBNull.Value : dto.q);
        command.Parameters.AddWithValue("category", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.Category) ? (object)DBNull.Value : dto.Category);
        command.Parameters.AddWithValue("parkId", NpgsqlDbType.Varchar,
            string.IsNullOrWhiteSpace(dto.ParkId) ? (object)DBNull.Value : dto.ParkId);
        // Add park IDs parameter
        command.Parameters.AddWithValue("parkIds", NpgsqlDbType.Array | NpgsqlDbType.Uuid,
            parkIds.Any() ? parkIds.ToArray() : (object)DBNull.Value);
    
        var jsonResult = await command.ExecuteScalarAsync() as string;
        if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get rare endangered species data");
        
        var apiResponse = JsonSerializer.Deserialize<RareEndangeredSpeciesSqlResponseDto>(
            jsonResult,
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            }
        );
    
        return new PagedList<RareEndangeredSpeciesResponseDto>(
            apiResponse!.Data,
            dto.page,
            dto.pageSize,
            apiResponse.Meta.TotalItems
        );
    }

    public async Task<string> Create(RareEndangeredSpeciesRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_rare_endangered_species(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetRareEndangeredSpeciesDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_rare_endangered_species_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get rare endangered species data");

        var result = JsonSerializer.Deserialize<GetRareEndangeredSpeciesDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetRareEndangeredSpeciesDto();
    }

    public async Task<string> Update(Guid id, RareEndangeredSpeciesRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_rare_endangered_species(@data::jsonb, @rareEndangeredSpeciesId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("rareEndangeredSpeciesId", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var rareEndangeredSpecies = await _context.RareEndangeredSpecies.FindAsync(id);
        if (rareEndangeredSpecies is null)
            throw new KeyNotFoundException("Rare endangered species record not found");

        rareEndangeredSpecies.DeletedAt = DateTime.UtcNow;
        rareEndangeredSpecies.UpdatedBy = _userContext.GetUserId();
        rareEndangeredSpecies.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}