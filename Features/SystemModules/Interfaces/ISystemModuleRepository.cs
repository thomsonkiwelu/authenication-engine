using conservation_backend.Shared;

namespace conservation_backend.Features.SystemModules.Interfaces;

public interface ISystemModuleRepository
{
    Task<PagedList<SystemModule>> GetPagedData(PaginationDto dto);

    Task<SystemModule> GetById(Guid id);
}