using conservation_backend.Config;
using conservation_backend.Features.LessRangerStations.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessRangerStations;

public class LessRangerStationRepository(AppDBContext context, IUserContext userContext) : ILessRangerStationRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<LessRangerStation>> GetPagedData(LessRangerStationPaginationDto dto)
    {
        string[] searchColumns = new[] { "Name" };

        var query = _context.LessRangerStations
            .Include(s => s.LessOperationalZone)
            .Include(s => s.Office)
            .Include(s => s.Creator)
            .AsNoTracking()
            .AsQueryable();

        query = ApplyFilters<LessRangerStation>.ApplySearch(query, dto.q ?? "", searchColumns);
        query = ApplyFilters<LessRangerStation>.ApplySorting(query, dto.sortBy, dto.sortDesc);

        if (!string.IsNullOrWhiteSpace(dto.LessOperationalZoneId))
            query = query.Where(s => s.LessOperationalZoneId == Guid.Parse(dto.LessOperationalZoneId));

        if (!string.IsNullOrWhiteSpace(dto.OfficeId))
            query = query.Where(s => s.OfficeId == Guid.Parse(dto.OfficeId));

        return await PagedList<LessRangerStation>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<LessRangerStation> Create(LessRangerStation station)
    {
        station.CreatedBy = _userContext.GetUserId();
        _context.LessRangerStations.Add(station);

        await _context.SaveChangesAsync();
        return station;
    }

    public async Task<LessRangerStation> GetById(Guid id)
    {
        var result = await _context.LessRangerStations
            .Include(s => s.LessOperationalZone)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (result is null)
            throw new KeyNotFoundException("Station record not found");

        return result;
    }

    public async Task<LessRangerStation> Update(Guid id, LessRangerStation station)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""LessRangerStations""
                SET ""Name"" = {station.Name}, ""Code"" = {station.Code}, ""LessOperationalZoneId"" = {station.LessOperationalZoneId}, ""OfficeId"" = {station.OfficeId},
                    ""UpdatedBy"" = {_userContext.GetUserId()}, ""UpdatedAt"" = {DateTime.UtcNow}
                WHERE ""Id"" = {id};
            ");

        if (rows == 0)
            throw new KeyNotFoundException("Station record not found.");

        var updated = await _context.LessRangerStations
            .Include(s => s.LessOperationalZone)
            .Include(s => s.Office)
            .FirstOrDefaultAsync(s => s.Id == id);

        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var station = await _context.LessRangerStations.FindAsync(id);

        if (station is null)
            throw new KeyNotFoundException("Station record not found");

        station.DeletedAt = DateTime.UtcNow;
        station.UpdatedBy = _userContext.GetUserId();
        station.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
