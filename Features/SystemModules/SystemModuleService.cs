using authentication_engine.Features.SystemModules.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.SystemModules;

public class SystemModuleService(ISystemModuleRepository repository, IMapper mapper): ISystemModuleService
{
    private readonly ISystemModuleRepository _systemModuleRepository = repository;

    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<SystemModuleResponseDto>> GetAllSystemModulesData(SystemModulePaginationDto dto)
    {
        var pagedData = await _systemModuleRepository.GetPagedData(dto);
        
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<SystemModuleResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();
        

        return new PagedList<SystemModuleResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }
    
    public async Task<SystemModuleDto> CreateSystemModule(SystemModuleRequestDto dto)
    {
        var systemModule = _mapper.Map<SystemModule>(dto);

        var created = await _systemModuleRepository.Create(systemModule);

        var responseDto = _mapper.Map<SystemModuleDto>(created);

        return responseDto;
    }

    public async Task<SystemModuleDto> GetSystemModuleById(Guid id)
    {
        var role = await _systemModuleRepository.GetById(id);

        var result = _mapper.Map<SystemModuleDto>(role);

        return result;
    }
    
    public async Task<SystemModuleDto> UpdateSystemModule(Guid id, SystemModuleRequestDto dto)
    {
        var systemModule = _mapper.Map<SystemModule>(dto);

        var updated = await _systemModuleRepository.Update(id, systemModule);

        var responseDto = _mapper.Map<SystemModuleDto>(updated);

        return responseDto;
    }
    
    public async Task<bool> DeleteSystemModule(Guid id)
    {
        return await _systemModuleRepository.Delete(id);
    }
}