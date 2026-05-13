using conservation_backend.Shared;

namespace conservation_backend.Features.VillageContexts.Interfaces;

public interface IVillageContextService
{
    Task<PagedList<VillageContextResponseDto>> GetAllVillageContexts(VillageContextPaginationDto dto);

    Task<VillageContextDto> CreateVillageContext(VillageContextRequestDto dto);

    Task<VillageContextDto> GetVillageContextById(Guid id);

    Task<VillageContextDto> UpdateVillageContext(Guid id, VillageContextRequestDto dto);

    Task<bool> DeleteVillageContext(Guid id);

    Task<GetVillageIssueDto> CreateVillageIssue(VillageIssueRequestDto dto);

    Task<GetVillageIssueDto> GetVillageIssueById(Guid id);
    
    Task<GetVillageIssueDto> UpdateVillageIssue(VillageIssueRequestDto dto);
}