using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.MangabeyObservations.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.MangabeyObservations;

public class MangabeyObservationRepository(AppDBContext context, IUserContext userContext): IMangabeyObservationRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<MangabeyObservation>> GetPagedData(MangabeyObservationPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Park.Name" };
        var query = _context.MangabeyObservations
            .Include(p => p.Park)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<MangabeyObservation>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<MangabeyObservation>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<MangabeyObservation>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<string> Create(MangabeyObservationRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_mangabey_observation(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetMangabeyObservationDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_mangabey_observation_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get mangabey observation data");

        var result = JsonSerializer.Deserialize<GetMangabeyObservationDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetMangabeyObservationDto();
    }

    public async Task<string> Update(Guid id, MangabeyObservationRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_mangabey_observation(@data::jsonb, @Id)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("Id", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var mangabeyObservation = await _context.MangabeyObservations.FindAsync(id);
        if (mangabeyObservation is null)
            throw new KeyNotFoundException("Failure to get mangabey observation data");

        mangabeyObservation.DeletedAt = DateTime.UtcNow;
        mangabeyObservation.UpdatedBy = _userContext.GetUserId();
        mangabeyObservation.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}