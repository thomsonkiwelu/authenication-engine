using conservation_backend.Config;
using conservation_backend.Features.WaterQuantities.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.WaterQuantities;

public class WaterQuantityRepository(AppDBContext context, IUserContext userContext): IWaterQuantityRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<WaterQuantity>> GetPagedData(WaterQuantityPaginationDto dto)
    {
        string[] searchColumns = new string[] { "WaterBody.Name" };
        var query = _context.WaterQuantities
            .Include(p => p.Park)
            .Include(w => w.WaterBody)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<WaterQuantity>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<WaterQuantity>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.WaterBodyId))
            query = query.Where(v => v.WaterBodyId == Guid.Parse(dto.WaterBodyId));

        return await PagedList<WaterQuantity>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<WaterQuantity> Create(WaterQuantity waterQuantity)
    {
        waterQuantity.CreatedBy = _userContext.GetUserId();
        
        var waterBody = await _context.WaterBodies.FindAsync(waterQuantity.WaterBodyId);
        waterQuantity.ParkId = waterBody!.ParkId;
        
        _context.WaterQuantities.Add(waterQuantity);
        await _context.SaveChangesAsync();
        return waterQuantity;
    }

    public async Task<WaterQuantity> GetById(Guid id)
    {
        var result = await _context.WaterQuantities
            .Include(p => p.Park)
            .Include(w => w.WaterBody)
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (result is null)
            throw new KeyNotFoundException($"No water quantity records found.");

        return result;
    }

    public async Task<WaterQuantity> Update(Guid id, WaterQuantity waterQuantity)
    {
        waterQuantity.UpdatedBy = _userContext.GetUserId();
        var waterBody = await _context.WaterBodies.FindAsync(waterQuantity.WaterBodyId);
        waterQuantity.ParkId = waterBody!.ParkId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""WaterQuantities""
            SET 
                ""WaterBodyId"" = {waterQuantity.WaterBodyId},
                ""ParkId"" = {waterQuantity.ParkId},
                ""WaterLevel"" = {waterQuantity.WaterLevel},
                ""WettedPerimeter"" = {waterQuantity.WettedPerimeter},
                ""WettedWidth"" = {waterQuantity.WettedWidth},
                ""AverageDepth"" = {waterQuantity.AverageDepth},
                ""Length"" = {waterQuantity.Length},
                ""AverageTime"" = {waterQuantity.AverageTime},
                ""MinimumFlowRate"" = {waterQuantity.MinimumFlowRate},
                ""MaximumFlowRate"" = {waterQuantity.MaximumFlowRate},
                ""AverageFlowRate"" = {waterQuantity.AverageFlowRate},
                ""CalculatedDischargeRate"" = {waterQuantity.CalculatedDischargeRate},
                ""Volume"" = {waterQuantity.Volume},
                ""Coordinate"" = {waterQuantity.Coordinate},
                ""Remark"" = {waterQuantity.Remark},
                ""UpdatedAt"" = {DateTime.UtcNow},
                ""UpdatedBy"" = {waterQuantity.UpdatedBy}
            WHERE ""Id"" = {id}
        ");
        
        if (rows == 0)
            throw new KeyNotFoundException($"Water quantity record not found.");
        
        var updated = await _context.WaterQuantities.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var waterQuantity = await _context.WaterQuantities.FindAsync(id);

        if (waterQuantity is null)
            throw new KeyNotFoundException("No water quantity records found.");

        waterQuantity.DeletedAt = DateTime.UtcNow;
        waterQuantity.UpdatedBy = _userContext.GetUserId();
        waterQuantity.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}