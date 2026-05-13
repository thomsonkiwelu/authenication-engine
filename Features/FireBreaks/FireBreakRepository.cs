using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.FireBreaks.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.FireBreaks;

public class FireBreakRepository(AppDBContext context, IUserContext userContext): IFireBreakRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<FireBreak>> GetPagedData(FireBreakPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Location.Name" };
        var query = _context.FireBreaks
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<FireBreak>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<FireBreak>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<FireBreak>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<string> Create(FireBreakRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_firebreak(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetFireBreakDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_firebreak_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get firebreak data");

        var result = JsonSerializer.Deserialize<GetFireBreakDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetFireBreakDto();
    }

    public async Task<string> Update(Guid id, FireBreakRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_firebreak(@data::jsonb, @Id)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("Id", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var fireBreak = await _context.FireBreaks.FindAsync(id);
        if (fireBreak is null)
            throw new KeyNotFoundException("Failure to get firebreak data");

        fireBreak.DeletedAt = DateTime.UtcNow;
        fireBreak.UpdatedBy = _userContext.GetUserId();
        fireBreak.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}