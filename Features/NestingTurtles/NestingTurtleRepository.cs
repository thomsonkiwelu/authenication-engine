using conservation_backend.Config;
using conservation_backend.Features.NestingTurtles.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.NestingTurtles;

public class NestingTurtleRepository(AppDBContext context, IUserContext userContext): INestingTurtleRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<NestingTurtle>> GetPagedData(NestingTurtlePaginationDto dto)
    {
        string[] searchColumns = new string[] { "Location.Name" };
        var query = _context.NestingTurtles
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<NestingTurtle>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<NestingTurtle>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<NestingTurtle>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<NestingTurtle> Create(NestingTurtle nestingTurtle)
    {
        nestingTurtle.CreatedBy = _userContext.GetUserId();
        
        var location = await _context.Locations.FindAsync(nestingTurtle.LocalAreaNameId);
        nestingTurtle.ParkId = location!.ParkId;
        
        _context.NestingTurtles.Add(nestingTurtle);
        await _context.SaveChangesAsync();
        
        return nestingTurtle;
    }

    public async Task<NestingTurtle> GetById(Guid id)
    {
        var nestingTurtle = await _context.NestingTurtles
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (nestingTurtle is null)
            throw new KeyNotFoundException($"Nesting Turtle record not found");

        return nestingTurtle;
    }

    public async Task<NestingTurtle> Update(Guid id, NestingTurtle nestingTurtle)
    {
        var location = await _context.Locations.FindAsync(nestingTurtle.LocalAreaNameId);
        nestingTurtle.ParkId = location!.ParkId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""NestingTurtles""
            SET ""LocalAreaNameId"" = {nestingTurtle.LocalAreaNameId}, ""HatchedEggs"" = {nestingTurtle.HatchedEggs},""UpdatedBy"" = {_userContext.GetUserId()},
            ""Hatchling"" = {nestingTurtle.Hatchling}, ""PoachedEggs"" = {nestingTurtle.PoachedEggs},""UnHatchedEggs"" = {nestingTurtle.UnHatchedEggs},
            ""ParkId"" = {nestingTurtle.ParkId},""Remark"" = {nestingTurtle.Remark},""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Nesting turtle record not found.");
            
        var updated = await _context.NestingTurtles.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var nestingTurtle = await _context.NestingTurtles.FindAsync(id);

        if (nestingTurtle is null)
            throw new KeyNotFoundException("Nesting turtle record not found");

        nestingTurtle.DeletedAt = DateTime.UtcNow;
        nestingTurtle.UpdatedBy = _userContext.GetUserId();
        nestingTurtle.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}