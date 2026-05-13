using authentication_engine.Shared;

namespace authentication_engine.Features.SystemModules.Interfaces;

public interface ISystemModuleService
{
    Task<PagedList<SystemModuleResponseDto>> GetAllSystemModulesData(SystemModulePaginationDto dto);
    
    Task<SystemModuleDto> CreateSystemModule(SystemModuleRequestDto dto);
    
    Task<SystemModuleDto> GetSystemModuleById(Guid id);
    
    Task<SystemModuleDto> UpdateSystemModule(Guid id, SystemModuleRequestDto dto);

    Task<bool> DeleteSystemModule(Guid id);
}