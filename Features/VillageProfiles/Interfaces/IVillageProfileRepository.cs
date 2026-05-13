using conservation_backend.Shared;

namespace conservation_backend.Features.VillageProfiles.Interfaces;

public interface IVillageProfileRepository
{
    Task<PagedList<VillageProfile>> GetPagedData(VillageProfilePaginationDto dto);

    Task<VillageProfile> Create(VillageProfile villageProfile);

    Task<VillageProfile> GetById(Guid id);

    Task<VillageProfile> Update(Guid id, VillageProfile villageProfile);

    Task<bool> Delete(Guid id);
    
    Task<GetVillageProfileDto> GetFullDetails(Guid id);
}