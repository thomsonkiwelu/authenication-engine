using conservation_backend.Config;
using conservation_backend.Features.Divisions.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Divisions;

public class DivisionRepository(AppDBContext context, IUserContext userContext) : IDivisionRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Division>> GetPagedData(DivisionPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Divisions
            .Include(r => r.Region)
            .Include(d => d.District)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Division>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Division>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.Name))
            query = query.Where(v => v.Name == dto.Name);
        
        if (!string.IsNullOrWhiteSpace(dto.RegionId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.RegionId));
        
        if (!string.IsNullOrWhiteSpace(dto.DistrictId))
            query = query.Where(v => v.RegionId == Guid.Parse(dto.DistrictId));

        return await PagedList<Division>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Division> Create(Division division)
    {
        division.CreatedBy = _userContext.GetUserId();
        
        var district = await _context.Districts.FindAsync(division.DistrictId);
        division.RegionId = district!.RegionId;
        
        _context.Divisions.Add(division);
        await _context.SaveChangesAsync();
        
        return division;
    }

    public async Task<Division> GetById(Guid id)
    {
        var division = await _context.Divisions
            .Include(r => r.Region)
            .Include(d => d.District)
            .Include(u => u.Creator)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (division is null)
            throw new KeyNotFoundException($"Division record not found");

        return division;
    }

    public async Task<Division> Update(Guid id, Division division)
    {
        var district = await _context.Districts.FindAsync(division.DistrictId);
        division.RegionId = district!.RegionId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Divisions""
            SET ""Name"" = {division.Name}, ""RegionId"" = {division.RegionId}, ""DistrictId"" = {division.DistrictId}, ""UpdatedBy"" = {_userContext.GetUserId()},
            ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
        
        if (rows == 0)
            throw new KeyNotFoundException("Division record not found.");
            
        var updated = await _context.Divisions.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var division = await _context.Divisions.FindAsync(id);
        if (division is null)
            throw new KeyNotFoundException("Division record not found");

        division.DeletedAt = DateTime.UtcNow;
        division.UpdatedBy = _userContext.GetUserId();
        division.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}