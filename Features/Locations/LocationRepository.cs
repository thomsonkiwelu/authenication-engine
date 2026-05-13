using conservation_backend.Config;
using conservation_backend.Features.Locations.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Locations;

public class LocationRepository(AppDBContext context, IUserContext userContext) : ILocationRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Location>> GetPagedData(LocationPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Locations
            .Include(p => p.Park)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Location>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Location>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<Location>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Location> Create(Location location)
    {
        location.CreatedBy = _userContext.GetUserId();
        _context.Locations.Add(location);

        await _context.SaveChangesAsync();
        return location;
    }

    public async Task<Location> GetById(Guid id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location is null)
            throw new KeyNotFoundException($"Location record not found");

        return location;
    }

    public async Task<Location> Update(Guid id, Location location)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Locations""
                SET ""Name"" = {location.Name}, ""ParkId"" = {location.ParkId},""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Location record not found.");

        var updatedLocation = await _context.Locations.FindAsync(id);
        return updatedLocation ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location is null)
            throw new KeyNotFoundException("Location record not found");

        location.DeletedAt = DateTime.UtcNow;
        location.UpdatedBy = _userContext.GetUserId();
        location.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}