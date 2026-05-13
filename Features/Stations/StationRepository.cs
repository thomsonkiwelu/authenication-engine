using conservation_backend.Config;
using conservation_backend.Features.Stations.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Stations;

public class StationRepository(AppDBContext context, IUserContext userContext) : IStationRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Station>> GetPagedData(StationPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Stations
            .Include(p => p.Park)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Station>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Station>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));
        
        if (dto.Category.HasValue)
            query = query.Where(v => v.Category == dto.Category);

        return await PagedList<Station>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Station> Create(Station station)
    {
        station.CreatedBy = _userContext.GetUserId();
        _context.Stations.Add(station);

        await _context.SaveChangesAsync();
        return station;
    }

    public async Task<Station> GetById(Guid id)
    {
        var result = await _context.Stations.FindAsync(id);
        
        if (result is null)
            throw new KeyNotFoundException($"Station record not found");

        return result;
    }

    public async Task<Station> Update(Guid id, Station station)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Stations""
                SET ""Name"" = {station.Name},""ParkId"" = {station.ParkId},""Type"" = {station.Type},""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Station record not found.");

        var updated = await _context.Stations.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var station = await _context.Stations.FindAsync(id);

        if (station is null)
            throw new KeyNotFoundException("Station record not found");

        station.DeletedAt = DateTime.UtcNow;
        station.UpdatedBy = _userContext.GetUserId();
        station.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
}