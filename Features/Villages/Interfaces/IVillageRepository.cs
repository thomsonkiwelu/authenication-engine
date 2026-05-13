using conservation_backend.Shared;

namespace conservation_backend.Features.Villages.Interfaces;

public interface IVillageRepository
{
    Task<PagedList<Village>> GetPagedData(VillagePaginationDto dto);

    Task<Village> Create(Village village);

    Task<Village> GetById(Guid id);

    Task<Village> Update(Guid id, Village village);

    Task<bool> Delete(Guid id);
}