using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.FirePrescriptions.Interfaces;
using conservation_backend.Features.WaterQuantities;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.FirePrescriptions;

public class FirePrescriptionRepository(AppDBContext context, IUserContext userContext): IFirePrescriptionRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<FirePrescription>> GetPagedData(FirePrescriptionPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Location.Name" };
        var query = _context.FirePrescriptions
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<FirePrescription>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<FirePrescription>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<FirePrescription>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<string> Create(FirePrescriptionRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_fire_prescription(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetFirePrescriptionDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_fire_prescription_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get fire prescription data");

        var result = JsonSerializer.Deserialize<GetFirePrescriptionDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetFirePrescriptionDto();
    }

    public async Task<string> Update(Guid id, FirePrescriptionRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_fire_prescription(@data::jsonb, @Id)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("Id", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var firePrescription = await _context.FirePrescriptions.FindAsync(id);
        if (firePrescription is null)
            throw new KeyNotFoundException("Failure to get fire prescription data");

        firePrescription.DeletedAt = DateTime.UtcNow;
        firePrescription.UpdatedBy = _userContext.GetUserId();
        firePrescription.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}