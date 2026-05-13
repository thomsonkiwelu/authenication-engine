using authentication_engine.Shared;

namespace authentication_engine.Features.SystemModules.Interfaces;

public interface ISystemModuleRepository
{
    Task<PagedList<SystemModule>> GetPagedData(PaginationDto dto);

    Task<SystemModule> GetById(Guid id);
}