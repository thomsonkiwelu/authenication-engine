using authentication_engine.Shared;

namespace authentication_engine.Features.Stations.Interfaces;

public interface IStationService
{
    Task<PagedList<StationResponseDto>> GetAllStationsData(StationPaginationDto dto);

    Task<StationDto> CreateStation(StationRequest dto);

    Task<StationDto> GetStationById(Guid id);

    Task<StationDto> UpdateStation(Guid id, StationRequest dto);

    Task<bool> DeleteStation(Guid id);
}