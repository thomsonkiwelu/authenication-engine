using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictContexts.Interfaces;

public interface IDistrictContextService
{
    Task<PagedList<DistrictContextResponseDto>> GetAllDistrictContexts(DistrictContextPaginationDto dto);

    Task<DistrictContextDto> CreateDistrictContext(DistrictContextRequestDto dto);

    Task<DistrictContextDto> GetDistrictContextById(Guid id);

    Task<DistrictContextDto> UpdateDistrictContext(Guid id, DistrictContextRequestDto dto);

    Task<bool> DeleteDistrictContext(Guid id);

    Task<DevelopmentOrganizationDto> CreateDevelopmentOrganization(DevelopmentOrganizationRequestDto dto);

    Task<DevelopmentOrganizationDto> UpdateDevelopmentOrganization(Guid id, DevelopmentOrganizationRequestDto dto);
    
    Task<bool> DeleteOrganization(Guid id);
}