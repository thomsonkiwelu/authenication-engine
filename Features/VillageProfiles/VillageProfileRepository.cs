using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.VillageProfiles.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.VillageProfiles;

public class VillageProfileRepository(AppDBContext context, IUserContext userContext): IVillageProfileRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<VillageProfile>> GetPagedData(VillageProfilePaginationDto dto)
    {
        string[] searchColumns = new string[] { "Village.Name" };
        var query = _context.VillageProfiles
            .Include(p => p.Park)
            .Include(v => v.Village)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<VillageProfile>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<VillageProfile>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<VillageProfile>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<VillageProfile> Create(VillageProfile villageProfile)
    {
        villageProfile.CreatedBy = _userContext.GetUserId();
        _context.VillageProfiles.Add(villageProfile);

        await _context.SaveChangesAsync();
        return villageProfile;
    }

    public async Task<VillageProfile> GetById(Guid id)
    {
        var villageProfile = await _context.VillageProfiles
            .Include(p => p.Park)
            .Include(v => v.Village)
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (villageProfile is null)
            throw new KeyNotFoundException($"Village profile record not found");

        return villageProfile;
    }

    public async Task<VillageProfile> Update(Guid id, VillageProfile villageProfile)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""VillageProfiles""
                SET ""VillageId"" = {villageProfile.VillageId}, ""ParkId"" = {villageProfile.ParkId}, ""VillageSize"" = {villageProfile.VillageSize}, 
                ""NumberOfHousehold"" = {villageProfile.NumberOfHousehold}, ""NumberOfCow"" = {villageProfile.NumberOfCow}, ""UpdatedBy"" = {_userContext.GetUserId()},
                ""NumberOfFemale"" = {villageProfile.NumberOfFemale}, ""NumberOfMale"" = {villageProfile.NumberOfMale}, ""NumberOfDog"" = {villageProfile.NumberOfDog},
                ""NumberOfSheep"" = {villageProfile.NumberOfSheep},""NumberOfGoat"" = {villageProfile.NumberOfGoat}, ""Population"" = {villageProfile.Population},
                ""LandStatus"" = {villageProfile.LandStatus},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Village profile record not found.");

        var updatedVillageProfile = await _context.VillageProfiles.FindAsync(id);
        return updatedVillageProfile ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var villageProfile = await _context.VillageProfiles.FindAsync(id);

        if (villageProfile is null)
            throw new KeyNotFoundException("Village profile record not found");

        villageProfile.DeletedAt = DateTime.UtcNow;
        villageProfile.UpdatedBy = _userContext.GetUserId();
        villageProfile.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<GetVillageProfileDto> GetFullDetails(Guid id)
    {
        var dataSql = "SELECT fn_village_profile_by_id({0})";
        var results = await _context.Database.SqlQueryRaw<string>(dataSql, id)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentNullException("Failure to get village profile data");

        var result = JsonSerializer.Deserialize<GetVillageProfileDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new GetVillageProfileDto();
    }
}