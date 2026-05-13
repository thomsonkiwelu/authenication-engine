using conservation_backend.Features.SystemModules;
using conservation_backend.Shared;

namespace conservation_backend.Features.SystemModule.Interfaces;

public interface ISystemModuleService
{
    Task<PagedList<SystemModuleDto>> GetAllSystemModulesData(PaginationDto dto);

    Task<SystemModuleDto> GetSystemModuleById(Guid id);
}