using conservation_backend.Config;
using conservation_backend.Features.Districts.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Districts;

public class DistrictRepository(AppDBContext context, IUserContext userContext) : IDistrictRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<District>> GetPagedData(DistrictPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Districts
            .Include(r => r.Region)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<District>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<District>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(v => v.Name == dto.Name);
        
        if (!string.IsNullOrWhiteSpace(dto.RegionId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.RegionId));

        return await PagedList<District>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<District> Create(District district)
    {
        district.CreatedBy = _userContext.GetUserId();
        _context.Districts.Add(district);

        await _context.SaveChangesAsync();
        return district;
    }

    public async Task<District> GetById(Guid id)
    {
        var district = await _context.Districts
            .Include(r => r.Region)
            .Include(u => u.Creator)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (district is null)
            throw new KeyNotFoundException($"District record not found");

        return district;
    }

    public async Task<District> Update(Guid id, District district)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Districts""
            SET ""Name"" = {district.Name}, ""RegionId"" = {district.RegionId}, ""UpdatedBy"" = {_userContext.GetUserId()},
            ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
            
        if (rows == 0)
            throw new KeyNotFoundException("District record not found.");
            
        var updated = await _context.Districts.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var district = await _context.Districts.FindAsync(id);
        if (district is null)
            throw new KeyNotFoundException("District record not found");

        district.DeletedAt = DateTime.UtcNow;
        district.UpdatedBy = _userContext.GetUserId();
        district.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}