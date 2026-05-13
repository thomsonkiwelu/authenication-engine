using conservation_backend.Config;
using conservation_backend.Features.WaterBodies;
using conservation_backend.Features.WaterQualities.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.WaterQualities;

public class WaterQualityRepository(AppDBContext context, IUserContext userContext): IWaterQualityRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<WaterQuality>> GetPagedData(WaterQualityPaginationDto dto)
    {
        string[] searchColumns = new string[] { "WaterBody.Name" };
        var query = _context.WaterQualities
            .Include(p => p.Park)
            .Include(w => w.WaterBody)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();
        
        //Apply search filter
        query = ApplyFilters<WaterQuality>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<WaterQuality>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.WaterBodyType))
            query = query.Where(v => v.WaterBody.Type == dto.WaterBodyType);

        return await PagedList<WaterQuality>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<WaterQuality> Create(WaterQuality waterQuality)
    {
        waterQuality.CreatedBy = _userContext.GetUserId();
        
        var waterBody = await _context.WaterBodies.FindAsync(waterQuality.WaterBodyId);
        waterQuality.ParkId = waterBody!.ParkId;
        
        _context.WaterQualities.Add(waterQuality);
        await _context.SaveChangesAsync();
        return waterQuality;
    }

    public async Task<WaterQuality> GetById(Guid id)
    {
        var result = await _context.WaterQualities
            .Include(p => p.Park)
            .Include(w => w.WaterBody)
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (result is null)
            throw new KeyNotFoundException($"No water quality records found.");

        return result;
    }

    public async Task<WaterQuality> Update(Guid id, WaterQuality waterQuality)
    {
        waterQuality.UpdatedBy = _userContext.GetUserId();
        var waterBody = await _context.WaterBodies.FindAsync(waterQuality.WaterBodyId);
        waterQuality.ParkId = waterBody!.ParkId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""WaterQualities""
            SET 
                ""WaterQualityType"" = {waterQuality.WaterQualityType},
                ""WaterBodyId"" = {waterQuality.WaterBodyId},
                ""ParkId"" = {waterQuality.ParkId},
                ""Temperature"" = {waterQuality.Temperature},
                ""AtmosphericPressure"" = {waterQuality.AtmosphericPressure},
                ""OxidationReductionPotential"" = {waterQuality.OxidationReductionPotential},
                ""DissolvedOxygenInPercentage"" = {waterQuality.DissolvedOxygenInPercentage},
                ""DissolvedOxygenInMg"" = {waterQuality.DissolvedOxygenInMg},
                ""TotalDissolvedSolid"" = {waterQuality.TotalDissolvedSolid},
                ""Resistivity"" = {waterQuality.Resistivity},
                ""SalinityInPpt"" = {waterQuality.SalinityInPpt},
                ""SalinityInPercentage"" = {waterQuality.SalinityInPercentage},
                ""Ssg"" = {waterQuality.Ssg},
                ""WaterFlowRate"" = {waterQuality.WaterFlowRate},
                ""FecalColiform"" = {waterQuality.FecalColiform},
                ""TotalColiform"" = {waterQuality.TotalColiform},
                ""PotentialOfHydrogen"" = {waterQuality.PotentialOfHydrogen},
                ""ElectricConductivity"" = {waterQuality.ElectricConductivity},
                ""Nitrate"" = {waterQuality.Nitrate},
                ""Fluoride"" = {waterQuality.Fluoride},
                ""Chloride"" = {waterQuality.Chloride},
                ""TotalAlkalinity"" = {waterQuality.TotalAlkalinity},
                ""Phosphate"" = {waterQuality.Phosphate},
                ""Turbidity"" = {waterQuality.Turbidity},
                ""Color"" = {waterQuality.Color},
                ""Settleable"" = {waterQuality.Settleable},
                ""TotalHardness"" = {waterQuality.TotalHardness},
                ""Calcium"" = {waterQuality.Calcium},
                ""Magnesium"" = {waterQuality.Magnesium},
                ""Iron"" = {waterQuality.Iron},
                ""Copper"" = {waterQuality.Copper},
                ""Chromium"" = {waterQuality.Chromium},
                ""Ammonia"" = {waterQuality.Ammonia},
                ""Nitrite"" = {waterQuality.Nitrite},
                ""Sulphate"" = {waterQuality.Sulphate},
                ""Sodium"" = {waterQuality.Sodium},
                ""Potassium"" = {waterQuality.Potassium},
                ""TotalSuspendedSolid"" = {waterQuality.TotalSuspendedSolid},
                ""Coordinate"" = {waterQuality.Coordinate},
                ""Remark"" = {waterQuality.Remark},
                ""UpdatedAt"" = {DateTime.UtcNow},
                ""UpdatedBy"" = {waterQuality.UpdatedBy}
            WHERE ""Id"" = {id}
        ");
        
        if (rows == 0)
            throw new KeyNotFoundException($"Water quality record not found.");
        
        var updated = await _context.WaterQualities.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var waterQuality = await _context.WaterQualities.FindAsync(id);

        if (waterQuality is null)
            throw new KeyNotFoundException("No water quality records found.");

        waterQuality.DeletedAt = DateTime.UtcNow;
        waterQuality.UpdatedBy = _userContext.GetUserId();
        waterQuality.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}