using authentication_engine.Features.SystemApplications;
using authentication_engine.Shared;

namespace authentication_engine.Features.SystemApplications.Interfaces
{
    public interface ISystemApplicationRepository
    {
        Task<PagedList<SystemApplication>> GetPagedData(PaginationDto dto);

        Task<SystemApplication> Create(SystemApplication systemApplication);

        Task<SystemApplication> GetById(Guid id);

        Task<SystemApplication> Update(Guid id, SystemApplication systemApplication);

        Task<bool> Delete(Guid id);
    }
}
