using conservation_backend.Features.SystemModule.Interfaces;
using conservation_backend.Features.SystemModules.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.SystemModules;

public class SystemModuleService(ISystemModuleRepository repository, IMapper mapper): ISystemModuleService
{
    private readonly ISystemModuleRepository _systemModuleRepository = repository;

    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<SystemModuleDto>> GetAllSystemModulesData(PaginationDto dto)
    {
        var pagedData = await _systemModuleRepository.GetPagedData(dto);
        
        var dtoList = _mapper.Map<List<SystemModuleDto>>(pagedData.Data).ToList();

        return new PagedList<SystemModuleDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<SystemModuleDto> GetSystemModuleById(Guid id)
    {
        var role = await _systemModuleRepository.GetById(id);

        var result = _mapper.Map<SystemModuleDto>(role);

        return result;
    }
}