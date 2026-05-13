namespace conservation_backend.Features.LessLivestockDailies.Interfaces;

public interface ILessLivestockDailyRepository
{
    Task<LessLivestockDaily?> GetByStationDate(Guid stationId, DateOnly dutyDate);

    Task<LessLivestockDaily> Create(LessLivestockDaily daily);

    Task SaveChanges();
}
