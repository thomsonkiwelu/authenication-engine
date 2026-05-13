using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.VillageContexts.Interfaces;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.VillageContexts;

public class VillageContextRepository(AppDBContext context, IUserContext userContext): IVillageContextRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<VillageContext>> GetPagedData(VillageContextPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.VillageContexts
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<VillageContext>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<VillageContext>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.EntityId))
            query = query.Where(v => v.EntityId == Guid.Parse(dto.EntityId));
        
        if (!string.IsNullOrWhiteSpace(dto.EntityName))
            query = query.Where(v => v.EntityName == dto.EntityName);

        return await PagedList<VillageContext>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<string> Create(VillageContextRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_village_context(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<VillageContextDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_village_context_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get village context data");

        var result = JsonSerializer.Deserialize<VillageContextDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new VillageContextDto();
    }

    public async Task<string> Update(Guid id, VillageContextRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_village_context(@data::jsonb, @villageContextId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("villageContextId", NpgsqlDbType.Varchar, id.ToString());
        
        var result = await command.ExecuteScalarAsync();
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var villageContext = await _context.VillageContexts.FindAsync(id);

        if (villageContext is null)
            throw new KeyNotFoundException("Village context record not found");

        villageContext.DeletedAt = DateTime.UtcNow;
        villageContext.UpdatedBy = _userContext.GetUserId();
        villageContext.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> CreateIssue(VillageIssueRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
    
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
    
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_village_issues(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
    
        var result = await command.ExecuteScalarAsync();
        return result != null && (bool)result;
    }
    
    public async Task<GetVillageIssueDto> GetIssuesById(Guid id)
    {
        var dataSql = "SELECT fn_village_issues_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get village issues data");

        var result = JsonSerializer.Deserialize<GetVillageIssueDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetVillageIssueDto();
    }
    
    public async Task<bool> UpdateIssue(VillageIssueRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
    
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_village_issues(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
    
        var result = await command.ExecuteScalarAsync();
        return result != null && (bool)result;
    }
    
}