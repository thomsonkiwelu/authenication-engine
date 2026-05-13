using authentication_engine.Shared;

namespace authentication_engine.Features.SystemModules.Interfaces;

public interface ISystemModuleService
{
    Task<PagedList<SystemModuleDto>> GetAllSystemModulesData(PaginationDto dto);

    Task<SystemModuleDto> GetSystemModuleById(Guid id);
}