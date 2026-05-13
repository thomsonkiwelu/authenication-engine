using conservation_backend.Shared;

namespace conservation_backend.Features.Stations.Interfaces;

public interface IStationRepository
{
    Task<PagedList<Station>> GetPagedData(StationPaginationDto dto);

    Task<Station> Create(Station station);

    Task<Station> GetById(Guid id);

    Task<Station> Update(Guid id, Station station);

    Task<bool> Delete(Guid id);
}