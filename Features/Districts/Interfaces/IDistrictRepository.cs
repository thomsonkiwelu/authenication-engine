using conservation_backend.Shared;

namespace conservation_backend.Features.Districts.Interfaces;

public interface IDistrictRepository
{
    Task<PagedList<District>> GetPagedData(DistrictPaginationDto dto);

    Task<District> Create(District district);

    Task<District> GetById(Guid id);

    Task<District> Update(Guid id, District district);

    Task<bool> Delete(Guid id);
}