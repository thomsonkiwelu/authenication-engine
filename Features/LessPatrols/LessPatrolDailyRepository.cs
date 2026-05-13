using conservation_backend.Config;
using conservation_backend.Features.LessPatrols.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessPatrols;

public class LessPatrolDailyRepository(AppDBContext context) : ILessPatrolDailyRepository
{
    private readonly AppDBContext _context = context;

    public Task<LessPatrolDaily?> GetByStationDate(Guid stationId, DateOnly dutyDate)
    {
        return _context.LessPatrolDailies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate);
    }

    public async Task<LessPatrolDaily> Create(LessPatrolDaily patrolDaily)
    {
        _context.LessPatrolDailies.Add(patrolDaily);
        await _context.SaveChangesAsync();
        return patrolDaily;
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}
