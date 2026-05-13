using conservation_backend.Shared;

namespace conservation_backend.Features.Regions.Interfaces;

public interface IRegionService
{
    Task<PagedList<RegionResponseDto>> GetAllRegions(RegionPaginationDto dto);

    Task<RegionDto> CreateRegion(RegionRequest dto);

    Task<RegionDto> GetRegionById(Guid id);

    Task<RegionDto> UpdateRegion(Guid id, RegionRequest dto);

    Task<bool> DeleteRegion(Guid id);
}