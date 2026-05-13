using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessRangerDailyDivisions.Interfaces;

public interface ILessRangerDailyDivisionRepository
{
    Task<LessRangerDailyDivision?> GetByStationDateCategory(Guid stationId, DateOnly dutyDate, string category);

    Task<LessRangerDailyDivision> Create(LessRangerDailyDivision division);

    Task<List<LessRangerDailyDivisionAssignment>> GetAssignments(Guid divisionId);

    Task<List<LessRangerDailyDivisionAssignment>> GetAssignmentsIncludingDeleted(Guid divisionId);

    Task SaveChanges();
}
