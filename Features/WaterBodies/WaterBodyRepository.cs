using conservation_backend.Config;
using conservation_backend.Features.WaterBodies.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.WaterBodies;

public class WaterBodyRepository(AppDBContext context, IUserContext userContext): IWaterBodyRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<WaterBody>> GetPagedData(WaterBodyPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.WaterBodies
            .Include(p => p.Park)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<WaterBody>.ApplySearch(query, dto.q ?? "", searchColumns);
        //Apply sorting filter
        query = ApplyFilters<WaterBody>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.Type))
            query = query.Where(v => v.Type == dto.Type);

        return await PagedList<WaterBody>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<WaterBody> Create(WaterBody waterBody)
    {
        waterBody.CreatedBy = _userContext.GetUserId();
        _context.WaterBodies.Add(waterBody);

        await _context.SaveChangesAsync();
        return waterBody;
    }

    public async Task<WaterBody> GetById(Guid id)
    {
        var result = await _context.WaterBodies.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException($"No water body records found.");

        return result;
    }

    public async Task<WaterBody> Update(Guid id, WaterBody waterBody)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""WaterBodies""
                SET ""Name"" = {waterBody.Name},""ParkId"" = {waterBody.ParkId},""Type"" = {waterBody.Type},""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("No water body records found.");

        var updated = await _context.WaterBodies.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var waterBody = await _context.WaterBodies.FindAsync(id);

        if (waterBody is null)
            throw new KeyNotFoundException("No water body records found.");

        waterBody.DeletedAt = DateTime.UtcNow;
        waterBody.UpdatedBy = _userContext.GetUserId();
        waterBody.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}