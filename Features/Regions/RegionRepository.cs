using conservation_backend.Config;
using conservation_backend.Features.Regions.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Regions;

public class RegionRepository(AppDBContext context, IUserContext userContext) : IRegionRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Region>> GetPagedData(RegionPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Regions
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Region>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Region>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(v => v.Name == dto.Name);

        return await PagedList<Region>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Region> Create(Region region)
    {
        region.CreatedBy = _userContext.GetUserId();
        _context.Regions.Add(region);

        await _context.SaveChangesAsync();
        return region;
    }

    public async Task<Region> GetById(Guid id)
    {
        var region = await _context.Regions.FindAsync(id);

        if (region is null)
            throw new KeyNotFoundException($"Region record not found");

        return region;
    }

    public async Task<Region> Update(Guid id, Region region)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Regions""
            SET ""Name"" = {region.Name}, ""UpdatedBy"" = {_userContext.GetUserId()},
            ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Region record not found.");
            
        var updated = await _context.Regions.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var region = await _context.Regions.FindAsync(id);
        if (region is null)
            throw new KeyNotFoundException("Region record not found");

        region.DeletedAt = DateTime.UtcNow;
        region.UpdatedBy = _userContext.GetUserId();
        region.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}