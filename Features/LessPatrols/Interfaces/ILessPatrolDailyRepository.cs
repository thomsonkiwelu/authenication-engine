using conservation_backend.Features.LessPatrols;
namespace conservation_backend.Features.LessPatrols.Interfaces;

public interface ILessPatrolDailyRepository
{
    Task<LessPatrolDaily?> GetByStationDate(Guid stationId, DateOnly dutyDate);

    Task<LessPatrolDaily> Create(LessPatrolDaily patrolDaily);

    Task SaveChanges();
}
