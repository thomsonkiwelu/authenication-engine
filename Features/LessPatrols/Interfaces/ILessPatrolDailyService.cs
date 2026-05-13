using conservation_backend.Shared;
using conservation_backend.Features.LessPatrols;

namespace conservation_backend.Features.LessPatrols.Interfaces;

public interface ILessPatrolDailyService
{
    Task<LessPatrolDailyResponseDto> GetEntry(LessPatrolDailyGetRequest request);

    Task<LessPatrolDailyResponseDto> SaveEntry(LessPatrolDailySaveRequest request);

    Task<PagedList<LessPatrolDailyHeaderDto>> GetHeaders(LessPatrolDailyHeadersRequest request);
}
