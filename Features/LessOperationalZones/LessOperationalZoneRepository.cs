using authentication_engine.Config;
using authentication_engine.Features.LessOperationalZones.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.LessOperationalZones;

public class LessOperationalZoneRepository(AppDBContext context, IUserContext userContext) : ILessOperationalZoneRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<LessOperationalZone>> GetPagedData(LessOperationalZonePaginationDto dto)
    {
        string[] searchColumns = new[] { "Name" };

        var query = _context.LessOperationalZones
            .Include(z => z.Park)
            .Include(z => z.Creator)
            .AsNoTracking()
            .AsQueryable();

        query = ApplyFilters<LessOperationalZone>.ApplySearch(query, dto.q ?? "", searchColumns);
        query = ApplyFilters<LessOperationalZone>.ApplySorting(query, dto.sortBy, dto.sortDesc);

        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(z => z.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<LessOperationalZone>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<LessOperationalZone> Create(LessOperationalZone zone)
    {
        zone.CreatedBy = _userContext.GetUserId();
        _context.LessOperationalZones.Add(zone);

        await _context.SaveChangesAsync();
        return zone;
    }

    public async Task<LessOperationalZone> GetById(Guid id)
    {
        var result = await _context.LessOperationalZones
            .Include(z => z.Park)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (result is null)
            throw new KeyNotFoundException("Operational zone record not found");

        return result;
    }

    public async Task<LessOperationalZone> Update(Guid id, LessOperationalZone zone)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""LessOperationalZones""
                SET ""Name"" = {zone.Name}, ""Code"" = {zone.Code}, ""ParkId"" = {zone.ParkId}, ""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");

        if (rows == 0)
            throw new KeyNotFoundException("Operational zone record not found.");

        var updated = await _context.LessOperationalZones
            .Include(z => z.Park)
            .FirstOrDefaultAsync(z => z.Id == id);

        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var zone = await _context.LessOperationalZones.FindAsync(id);

        if (zone is null)
            throw new KeyNotFoundException("Operational zone record not found");

        zone.DeletedAt = DateTime.UtcNow;
        zone.UpdatedBy = _userContext.GetUserId();
        zone.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
