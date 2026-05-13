using conservation_backend.Config;
using conservation_backend.Features.MangabeyMonitoring.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.MangabeyMonitoring;

public class MangabeyMonitoringRepository(AppDBContext context, IUserContext userContext): IMangabeyMonitoringRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<MangabeyMonitoring>> GetPagedData(MangabeyMonitoringPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Park.Name" };
        var query = _context.MangabeyMonitoring
            .Include(p => p.Park)
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<MangabeyMonitoring>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<MangabeyMonitoring>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.ParkId));
        
        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(v => v.ParkId == Guid.Parse(dto.ParkId));

        return await PagedList<MangabeyMonitoring>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<MangabeyMonitoring> Create(MangabeyMonitoring mangabeyMonitoring)
    {
        mangabeyMonitoring.CreatedBy = _userContext.GetUserId();
        _context.MangabeyMonitoring.Add(mangabeyMonitoring);

        await _context.SaveChangesAsync();
        return mangabeyMonitoring;
    }

    public async Task<MangabeyMonitoring> GetById(Guid id)
    {
        var mangabeyMonitoring = await _context.MangabeyMonitoring
            .Include(p => p.Park)
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (mangabeyMonitoring is null)
            throw new KeyNotFoundException($"Mangabey monitoring record not found");

        return mangabeyMonitoring;
    }

    public async Task<MangabeyMonitoring> Update(Guid id, MangabeyMonitoring mangabeyMonitoring)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""MangabeyMonitoring""
                SET ""ActivityType"" = {mangabeyMonitoring.ActivityType}, ""NumberOfParticipant"" = {mangabeyMonitoring.NumberOfParticipant}, ""NumberOfSpecies"" = {mangabeyMonitoring.NumberOfSpecies}, 
                ""ParkId"" = {mangabeyMonitoring.ParkId}, ""Coordinates"" = {mangabeyMonitoring.Coordinates}, ""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Mangabey monitoring record not found.");

        var updatedMangabeyMonitoring = await _context.MangabeyMonitoring.FindAsync(id);
        return updatedMangabeyMonitoring ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var mangabeyMonitoring = await _context.MangabeyMonitoring.FindAsync(id);

        if (mangabeyMonitoring is null)
            throw new KeyNotFoundException("Mangabey monitoring record not found");

        mangabeyMonitoring.DeletedAt = DateTime.UtcNow;
        mangabeyMonitoring.UpdatedBy = _userContext.GetUserId();
        mangabeyMonitoring.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}