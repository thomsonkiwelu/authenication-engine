using conservation_backend.Config;
using conservation_backend.Features.Wards.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Wards;

public class WardRepository(AppDBContext context, IUserContext userContext) : IWardRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Ward>> GetPagedData(WardPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Wards
            .Include(d => d.District)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Ward>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Ward>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(v => v.Name == dto.Name);
        
        if (!string.IsNullOrWhiteSpace(dto.RegionId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.RegionId));
        
        if (!string.IsNullOrWhiteSpace(dto.DistrictId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.DistrictId));
        
        if (!string.IsNullOrWhiteSpace(dto.DivisionId))
            query = query.Where(v => v.DivisionId == Guid.Parse(dto.DivisionId));

        return await PagedList<Ward>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Ward> Create(Ward ward)
    {
        ward.CreatedBy = _userContext.GetUserId();
        
        var district = await _context.Districts.FindAsync(ward.DistrictId);
        ward.RegionId = district!.RegionId;
        
        _context.Wards.Add(ward);
        await _context.SaveChangesAsync();
        
        return ward;
    }

    public async Task<Ward> GetById(Guid id)
    {
        var ward = await _context.Wards
            .Include(r => r.Region)
            .Include(d => d.District)
            .Include(s => s.Division)
            .Include(u => u.Creator)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (ward is null)
            throw new KeyNotFoundException($"Ward record not found");

        return ward;
    }

    public async Task<Ward> Update(Guid id, Ward ward)
    {
        var district = await _context.Districts.FindAsync(ward.DistrictId);
        ward.RegionId = district!.RegionId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Wards""
            SET ""Name"" = {ward.Name}, ""RegionId"" = {ward.RegionId}, ""DistrictId"" = {ward.DistrictId}, ""UpdatedBy"" = {_userContext.GetUserId()},
            ""DivisionId"" = {ward.DivisionId}, ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
        
        if (rows == 0)
            throw new KeyNotFoundException("Ward record not found.");
            
        var updated = await _context.Wards.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var ward = await _context.Wards.FindAsync(id);
        if (ward is null)
            throw new KeyNotFoundException("Ward record not found");

        ward.DeletedAt = DateTime.UtcNow;
        ward.UpdatedBy = _userContext.GetUserId();
        ward.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}