using conservation_backend.Shared;

namespace conservation_backend.Features.Locations.Interfaces;

public interface ILocationService
{
    Task<PagedList<LocationResponseDto>> GetAllLocationsData(LocationPaginationDto dto);

    Task<LocationDto> CreateLocation(LocationRequest dto);

    Task<LocationDto> GetLocationById(Guid id);

    Task<LocationDto> UpdateLocation(Guid id, LocationRequest dto);

    Task<bool> DeleteLocation(Guid id);
    
}