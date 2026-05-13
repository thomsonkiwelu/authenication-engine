using conservation_backend.Shared;

namespace conservation_backend.Features.Regions.Interfaces;

public interface IRegionRepository
{
    Task<PagedList<Region>> GetPagedData(RegionPaginationDto dto);

    Task<Region> Create(Region region);

    Task<Region> GetById(Guid id);

    Task<Region> Update(Guid id, Region region);

    Task<bool> Delete(Guid id);
}