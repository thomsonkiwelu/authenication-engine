using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.GroundCounts.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.GroundCounts;

public class GroundCountRepository(AppDBContext context, IUserContext userContext): IGroundCountRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<GroundCount>> GetPagedData(GroundCountPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Location.Name" };
        var query = _context.GroundCounts
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<GroundCount>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<GroundCount>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.Method))
            query = query.Where(v => v.Method == dto.Method);

        return await PagedList<GroundCount>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<string> Create(GroundCountRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_ground_count(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetGroundCountDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_ground_count_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get ground count data");

        var result = JsonSerializer.Deserialize<GetGroundCountDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetGroundCountDto();
    }

    public async Task<string> Update(Guid id, GroundCountRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_ground_count(@data::jsonb, @groundCountId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("groundCountId", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var groundCount = await _context.GroundCounts.FindAsync(id);
        if (groundCount is null)
            throw new KeyNotFoundException("Ground count record not found");

        groundCount.DeletedAt = DateTime.UtcNow;
        groundCount.UpdatedBy = _userContext.GetUserId();
        groundCount.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}