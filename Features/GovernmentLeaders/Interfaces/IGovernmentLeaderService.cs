using conservation_backend.Shared;

namespace conservation_backend.Features.GovernmentLeaders.Interfaces;

public interface IGovernmentLeaderService
{
    Task<PagedList<GovernmentLeaderResponseDto>> GetAllGovernmentLeaders(GovernmentLeaderPaginationDto dto);

    Task<GovernmentLeaderDto> CreateGovernmentLeader(GovernmentLeaderRequestDto dto);

    Task<GovernmentLeaderDto> GetGovernmentLeaderById(Guid id);

    Task<GovernmentLeaderDto> UpdateGovernmentLeader(Guid id, GovernmentLeaderRequestDto dto);

    Task<bool> DeleteGovernmentLeader(Guid id);
}