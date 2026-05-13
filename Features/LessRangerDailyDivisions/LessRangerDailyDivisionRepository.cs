using conservation_backend.Config;
using conservation_backend.Features.LessRangerDailyDivisions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessRangerDailyDivisions;

public class LessRangerDailyDivisionRepository(AppDBContext context) : ILessRangerDailyDivisionRepository
{
    private readonly AppDBContext _context = context;

    public Task<LessRangerDailyDivision?> GetByStationDateCategory(Guid stationId, DateOnly dutyDate, string category)
    {
        return _context.LessRangerDailyDivisions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate && x.Category == category);
    }

    public async Task<LessRangerDailyDivision> Create(LessRangerDailyDivision division)
    {
        _context.LessRangerDailyDivisions.Add(division);
        await _context.SaveChangesAsync();
        return division;
    }

    public Task<List<LessRangerDailyDivisionAssignment>> GetAssignments(Guid divisionId)
    {
        return _context.LessRangerDailyDivisionAssignments
            .Include(x => x.Staff)
            .ThenInclude(s => s.Rank)
            .Where(x => x.LessRangerDailyDivisionId == divisionId)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<LessRangerDailyDivisionAssignment>> GetAssignmentsIncludingDeleted(Guid divisionId)
    {
        return _context.LessRangerDailyDivisionAssignments
            .IgnoreQueryFilters()
            .Where(x => x.LessRangerDailyDivisionId == divisionId)
            .ToListAsync();
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}
