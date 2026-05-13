using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictContexts.Interfaces;

public interface IDistrictContextRepository
{
    Task<PagedList<DistrictContext>> GetPagedData(DistrictContextPaginationDto dto);

    Task<DistrictContext> Create(DistrictContext districtContext);

    Task<DistrictContext> GetById(Guid id);

    Task<DistrictContext> Update(Guid id, DistrictContext districtContext);

    Task<bool> Delete(Guid id);

    Task<DevelopmentOrganization> CreateOrganization(DevelopmentOrganization developmentOrganization);

    Task<DevelopmentOrganization> UpdateOrganization(Guid id, DevelopmentOrganization developmentOrganization);
    
    Task<bool> DeleteOrganization(Guid id);
}