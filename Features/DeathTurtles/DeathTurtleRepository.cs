using conservation_backend.Config;
using conservation_backend.Features.DeathTurtles.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.DeathTurtles;

public class DeathTurtleRepository(AppDBContext context, IUserContext userContext): IDeathTurtleRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<DeathTurtle>> GetPagedData(DeathTurtlePaginationDto dto)
    {
        string[] searchColumns = new string[] { "Location.Name" };
        var query = _context.DeathTurtles
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<DeathTurtle>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<DeathTurtle>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<DeathTurtle>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<DeathTurtle> Create(DeathTurtle deathTurtle)
    {
        deathTurtle.CreatedBy = _userContext.GetUserId();
        
        var location = await _context.Locations.FindAsync(deathTurtle.LocalAreaNameId);
        deathTurtle.ParkId = location!.ParkId;
        
        _context.DeathTurtles.Add(deathTurtle);
        await _context.SaveChangesAsync();
        
        return deathTurtle;
    }

    public async Task<DeathTurtle> GetById(Guid id)
    {
        var deathTurtle = await _context.DeathTurtles
            .Include(p => p.Park)
            .Include(l => l.Location)
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (deathTurtle is null)
            throw new KeyNotFoundException($"Death Turtle record not found");

        return deathTurtle;
    }

    public async Task<DeathTurtle> Update(Guid id, DeathTurtle deathTurtle)
    {
        var location = await _context.Locations.FindAsync(deathTurtle.LocalAreaNameId);
        deathTurtle.ParkId = location!.ParkId;
        
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""DeathTurtles""
            SET ""LocalAreaNameId"" = {deathTurtle.LocalAreaNameId}, ""CauseOfDeath"" = {deathTurtle.CauseOfDeath},""UpdatedBy"" = {_userContext.GetUserId()},
            ""DeadAdults"" = {deathTurtle.DeadAdults}, ""DeadHatchings"" = {deathTurtle.DeadHatchings},""Coordinates"" = {deathTurtle.Coordinates},
            ""ParkId"" = {deathTurtle.ParkId},""Remark"" = {deathTurtle.Remark},""Reason"" = {deathTurtle.Reason},""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Death turtle record not found.");
            
        var updated = await _context.DeathTurtles.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var deathTurtle = await _context.DeathTurtles.FindAsync(id);

        if (deathTurtle is null)
            throw new KeyNotFoundException("Death turtle record not found");

        deathTurtle.DeletedAt = DateTime.UtcNow;
        deathTurtle.UpdatedBy = _userContext.GetUserId();
        deathTurtle.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}