using conservation_backend.Features.VillageContexts.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.VillageContexts;

public class VillageContextService(IVillageContextRepository repository, IMapper mapper): IVillageContextService
{
    private readonly IVillageContextRepository _villageContextRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<VillageContextResponseDto>> GetAllVillageContexts(VillageContextPaginationDto dto)
    {
        var pagedData = await _villageContextRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<VillageContextResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<VillageContextResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<VillageContextDto> CreateVillageContext(VillageContextRequestDto dto)
    {
        var villageContextId = await _villageContextRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(villageContextId))
            throw new ArgumentNullException("Failure to create village context data");
        
        return await _villageContextRepository.GetById(Guid.Parse(villageContextId));
    }

    public async Task<VillageContextDto> GetVillageContextById(Guid id)
    {
        return await _villageContextRepository.GetById(id);
    }

    public async Task<VillageContextDto> UpdateVillageContext(Guid id, VillageContextRequestDto dto)
    {
        var villageContextId = await _villageContextRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(villageContextId))
            throw new ArgumentNullException("Failure to update village context data");
        
        return await _villageContextRepository.GetById(Guid.Parse(villageContextId));
    }

    public async Task<bool> DeleteVillageContext(Guid id)
    {
        return await _villageContextRepository.Delete(id);
    }
    
    public async Task<GetVillageIssueDto> CreateVillageIssue(VillageIssueRequestDto dto)
    {
        var result = await _villageContextRepository.CreateIssue(dto);
        
        if (!result)
            throw new ArgumentNullException("Failure to create village issue data");
        
        return await _villageContextRepository.GetIssuesById(dto.EntityId);
    }
    
    public async Task<GetVillageIssueDto> GetVillageIssueById(Guid id)
    {
        return await _villageContextRepository.GetIssuesById(id);
    }
    
    public async Task<GetVillageIssueDto> UpdateVillageIssue(VillageIssueRequestDto dto)
    {
        var result = await _villageContextRepository.UpdateIssue(dto);
        
        if (!result)
            throw new ArgumentNullException("Failure to update village issue data");
        
        return await _villageContextRepository.GetIssuesById(dto.EntityId);
    }
    
}