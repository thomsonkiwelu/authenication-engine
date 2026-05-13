using conservation_backend.Shared;

namespace conservation_backend.Features.GovernmentLeaders.Interfaces;

public interface IGovernmentLeaderRepository
{
    Task<PagedList<GovernmentLeader>> GetPagedData(GovernmentLeaderPaginationDto dto);

    Task<GovernmentLeader> Create(GovernmentLeader governmentLeader);

    Task<GovernmentLeader> GetById(Guid id);

    Task<GovernmentLeader> Update(Guid id, GovernmentLeader governmentLeader);

    Task<bool> Delete(Guid id);
}