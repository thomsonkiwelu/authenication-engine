using conservation_backend.Shared;

namespace conservation_backend.Features.Locations.Interfaces;

public interface ILocationRepository
{
    Task<PagedList<Location>> GetPagedData(LocationPaginationDto dto);

    Task<Location> Create(Location location);

    Task<Location> GetById(Guid id);

    Task<Location> Update(Guid id, Location location);

    Task<bool> Delete(Guid id);
}