using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerStations.Interfaces;

public interface ILessRangerStationRepository
{
    Task<PagedList<LessRangerStation>> GetPagedData(LessRangerStationPaginationDto dto);

    Task<LessRangerStation> Create(LessRangerStation station);

    Task<LessRangerStation> GetById(Guid id);

    Task<LessRangerStation> Update(Guid id, LessRangerStation station);

    Task<bool> Delete(Guid id);
}
