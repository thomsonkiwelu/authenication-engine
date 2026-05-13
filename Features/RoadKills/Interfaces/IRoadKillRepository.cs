using conservation_backend.Shared;

namespace conservation_backend.Features.RoadKills.Interfaces;

public interface IRoadKillRepository
{
    Task<PagedList<RoadKillResponseDto>> GetPagedData(RoadKillPaginationDto dto);

    Task<string> Create(RoadKillRequestDto dto);

    Task<GetRoadKillDto> GetById(Guid id);

    Task<string> Update(Guid id, RoadKillRequestDto dto);

    Task<bool> Delete(Guid id);
}