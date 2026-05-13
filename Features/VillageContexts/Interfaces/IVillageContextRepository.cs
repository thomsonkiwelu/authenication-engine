using conservation_backend.Shared;

namespace conservation_backend.Features.VillageContexts.Interfaces;

public interface IVillageContextRepository
{
    Task<PagedList<VillageContext>> GetPagedData(VillageContextPaginationDto dto);

    Task<string> Create(VillageContextRequestDto dto);

    Task<VillageContextDto> GetById(Guid id);

    Task<string> Update(Guid id, VillageContextRequestDto dto);

    Task<bool> Delete(Guid id);
    
    Task<bool> CreateIssue(VillageIssueRequestDto dto);

    Task<GetVillageIssueDto> GetIssuesById(Guid id);
    
    Task<bool> UpdateIssue(VillageIssueRequestDto dto);
}