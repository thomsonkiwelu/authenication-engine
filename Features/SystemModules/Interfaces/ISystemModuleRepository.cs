using authentication_engine.Shared;

namespace authentication_engine.Features.SystemModules.Interfaces;

public interface ISystemModuleRepository
{
    Task<PagedList<SystemModule>> GetPagedData(SystemModulePaginationDto dto);
    
    Task<SystemModule> Create(SystemModule systemModule);
    
    Task<SystemModule> GetById(Guid id);

    Task<SystemModule> Update(Guid id, SystemModule systemModule);

    Task<bool> Delete(Guid id);
}