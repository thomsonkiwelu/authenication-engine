using conservation_backend.Features.DistrictContexts.Interfaces;
using conservation_backend.Features.DistrictProfiles.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.DistrictContexts;

public class DistrictContextService(IDistrictContextRepository repository, IMapper mapper) : IDistrictContextService
{
    private readonly IDistrictContextRepository _districtContextRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<DistrictContextResponseDto>> GetAllDistrictContexts(DistrictContextPaginationDto dto)
    {
        var pagedData = await _districtContextRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<DistrictContextResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<DistrictContextResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<DistrictContextDto> CreateDistrictContext(DistrictContextRequestDto dto)
    {
        var districtContext = _mapper.Map<DistrictContext>(dto);
        var created = await _districtContextRepository.Create(districtContext);
        
        var responseDto = _mapper.Map<DistrictContextDto>(created);
        return responseDto;
    }

    public async Task<DistrictContextDto> GetDistrictContextById(Guid id)
    {
        var districtContext = await _districtContextRepository.GetById(id);
        
        var result = _mapper.Map<DistrictContextDto>(districtContext);
        
        return result;
    }

    public async Task<DistrictContextDto> UpdateDistrictContext(Guid id, DistrictContextRequestDto dto)
    {
        var districtContext = _mapper.Map<DistrictContext>(dto);

        var updated = await _districtContextRepository.Update(id, districtContext);

        var responseDto = _mapper.Map<DistrictContextDto>(updated);

        return responseDto;
    }

    public async Task<bool> DeleteDistrictContext(Guid id)
    {
        return await _districtContextRepository.Delete(id);
    }
    
    public async Task<DevelopmentOrganizationDto> CreateDevelopmentOrganization(DevelopmentOrganizationRequestDto dto)
    {
        var developmentOrganization = _mapper.Map<DevelopmentOrganization>(dto);
        var created = await _districtContextRepository.CreateOrganization(developmentOrganization);
        
        var responseDto = _mapper.Map<DevelopmentOrganizationDto>(created);
        return responseDto;
    }
    
    public async Task<DevelopmentOrganizationDto> UpdateDevelopmentOrganization(Guid id, DevelopmentOrganizationRequestDto dto)
    {
        var developmentOrganization = _mapper.Map<DevelopmentOrganization>(dto);

        var updated = await _districtContextRepository.UpdateOrganization(id, developmentOrganization);

        var responseDto = _mapper.Map<DevelopmentOrganizationDto>(updated);

        return responseDto;
    }
    
    public async Task<bool> DeleteOrganization(Guid id)
    {
        return await _districtContextRepository.DeleteOrganization(id);
    }
}