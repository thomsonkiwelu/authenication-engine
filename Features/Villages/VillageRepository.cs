using conservation_backend.Config;
using conservation_backend.Features.Villages.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Villages;

public class VillageRepository(AppDBContext context, IUserContext userContext) : IVillageRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Village>> GetPagedData(VillagePaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Villages
            .Include(w => w.Ward)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Village>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Village>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(v => v.Name == dto.Name);
        
        if (!string.IsNullOrWhiteSpace(dto.RegionId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.RegionId));
        
        if (!string.IsNullOrWhiteSpace(dto.DistrictId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.DistrictId));
        
        if (!string.IsNullOrWhiteSpace(dto.DivisionId))
            query = query.Where(v => v.DivisionId == Guid.Parse(dto.DivisionId));
        
        if (!string.IsNullOrWhiteSpace(dto.WardId))
            query = query.Where(v => v.WardId == Guid.Parse(dto.WardId));

        return await PagedList<Village>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Village> Create(Village village)
    {
        village.CreatedBy = _userContext.GetUserId();
        
        var ward = await _context.Wards.FindAsync(village.WardId);
        village.RegionId = ward!.RegionId;
        village.DistrictId = ward!.DistrictId;
        
        _context.Villages.Add(village);
        await _context.SaveChangesAsync();
        
        return village;
    }

    public async Task<Village> GetById(Guid id)
    {
        var village = await _context.Villages
            .Include(r => r.Region)
            .Include(d => d.District)
            .Include(s => s.Division)
            .Include(w => w.Ward)
            .Include(u => u.Creator)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (village is null)
            throw new KeyNotFoundException($"Village record not found");

        return village;
    }

    public async Task<Village> Update(Guid id, Village village)
    {
        var ward = await _context.Wards.FindAsync(village.WardId);
        village.RegionId = ward!.RegionId;
        village.DistrictId = ward!.DistrictId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Villages""
            SET ""Name"" = {village.Name}, ""RegionId"" = {village.RegionId}, ""DistrictId"" = {village.DistrictId}, ""UpdatedBy"" = {_userContext.GetUserId()},
            ""DivisionId"" = {village.DivisionId}, ""WardId"" = {village.WardId}, ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
        
        if (rows == 0)
            throw new KeyNotFoundException("Village record not found.");
            
        var updated = await _context.Villages.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var village = await _context.Villages.FindAsync(id);
        if (village is null)
            throw new KeyNotFoundException("Village record not found");

        village.DeletedAt = DateTime.UtcNow;
        village.UpdatedBy = _userContext.GetUserId();
        village.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}