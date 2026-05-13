using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerStations.Interfaces;

public interface ILessRangerStationService
{
    Task<PagedList<LessRangerStationResponseDto>> GetAllStationsData(LessRangerStationPaginationDto dto);

    Task<LessRangerStationDto> CreateStation(LessRangerStationRequest dto);

    Task<LessRangerStationDto> GetStationById(Guid id);

    Task<LessRangerStationDto> UpdateStation(Guid id, LessRangerStationRequest dto);

    Task<bool> DeleteStation(Guid id);
}
