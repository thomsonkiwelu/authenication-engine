using authentication_engine.Shared;

namespace authentication_engine.Features.LessOperationalZones.Interfaces;

public interface ILessOperationalZoneService
{
    Task<PagedList<LessOperationalZoneResponseDto>> GetAllZonesData(LessOperationalZonePaginationDto dto);

    Task<LessOperationalZoneDto> CreateZone(LessOperationalZoneRequest dto);

    Task<LessOperationalZoneDto> GetZoneById(Guid id);

    Task<LessOperationalZoneDto> UpdateZone(Guid id, LessOperationalZoneRequest dto);

    Task<bool> DeleteZone(Guid id);
}
