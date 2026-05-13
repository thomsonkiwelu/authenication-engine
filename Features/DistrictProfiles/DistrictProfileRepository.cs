using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.DistrictProfiles.Interfaces;
using conservation_backend.Features.VillageProfiles;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace conservation_backend.Features.DistrictProfiles;

public class DistrictProfileRepository(AppDBContext context, IUserContext userContext): IDistrictProfileRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<DistrictProfile>> GetPagedData(DistrictProfilePaginationDto dto)
    {
        string[] searchColumns = new string[] { "District.Name" };
        var query = _context.DistrictProfiles
            .Include(d => d.District)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<DistrictProfile>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<DistrictProfile>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<DistrictProfile>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<string> Create(DistrictProfileRequestDto dto)
    {
        dto.CreatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_create_district_profile(@data::jsonb)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<GetDistrictProfileDto> GetById(Guid id)
    {
        var dataSql = "SELECT fn_district_profile_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get district profile data");

        var result = JsonSerializer.Deserialize<GetDistrictProfileDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetDistrictProfileDto();
    }

    public async Task<string> Update(Guid id, DistrictProfileRequestDto dto)
    {
        dto.UpdatedBy = _userContext.GetUserId();
        var dtoToJson = JsonSerializer.Serialize(dto);
            
        await using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(
            "SELECT fn_update_district_profile(@data::jsonb, @districtProfileId)",
            connection
        );
        command.Parameters.AddWithValue("data", NpgsqlDbType.Jsonb, dtoToJson);
        command.Parameters.AddWithValue("districtProfileId", NpgsqlDbType.Varchar, id.ToString());
        var result = await command.ExecuteScalarAsync();
        
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> Delete(Guid id)
    {
        var districtProfile = await _context.DistrictProfiles.FindAsync(id);
        if (districtProfile is null)
            throw new KeyNotFoundException("District profile record not found");

        districtProfile.DeletedAt = DateTime.UtcNow;
        districtProfile.UpdatedBy = _userContext.GetUserId();
        districtProfile.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<GetFullDistrictProfileDto> GetFullDetails(Guid id)
    {
        var dataSql = "SELECT fn_district_profile_details({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get district profile data");

        var result = JsonSerializer.Deserialize<GetFullDistrictProfileDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetFullDistrictProfileDto();
    }
}