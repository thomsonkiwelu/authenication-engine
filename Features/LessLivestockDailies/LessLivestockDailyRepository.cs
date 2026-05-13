using conservation_backend.Config;
using conservation_backend.Features.LessLivestockDailies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessLivestockDailies;

public class LessLivestockDailyRepository(AppDBContext context) : ILessLivestockDailyRepository
{
    private readonly AppDBContext _context = context;

    public Task<LessLivestockDaily?> GetByStationDate(Guid stationId, DateOnly dutyDate)
    {
        return _context.LessLivestockDailies
            .AsNoTracking()
            .Include(x => x.Livestock)
            .Include(x => x.Actions)
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate);
    }

    public async Task<LessLivestockDaily> Create(LessLivestockDaily daily)
    {
        _context.LessLivestockDailies.Add(daily);
        await _context.SaveChangesAsync();
        return daily;
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}
