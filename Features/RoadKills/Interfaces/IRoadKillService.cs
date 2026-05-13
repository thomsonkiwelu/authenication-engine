using conservation_backend.Shared;

namespace conservation_backend.Features.RoadKills.Interfaces;

public interface IRoadKillService
{
    Task<PagedList<RoadKillResponseDto>> GetPagedRoadKills(RoadKillPaginationDto dto);
    
    Task<GetRoadKillDto> CreateRoadKill(RoadKillRequestDto dto);
    
    Task<GetRoadKillDto> GetRoadKillById(Guid id);

    Task<GetRoadKillDto> UpdateRoadKill(Guid id , RoadKillRequestDto dto);

    Task<bool> DeleteRoadKill(Guid id);
}