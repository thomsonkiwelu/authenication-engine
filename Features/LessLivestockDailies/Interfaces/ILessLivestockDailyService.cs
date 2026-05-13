using conservation_backend.Shared;

namespace conservation_backend.Features.LessLivestockDailies.Interfaces;

public interface ILessLivestockDailyService
{
    Task<LessLivestockDailyResponseDto> GetEntry(LessLivestockDailyGetRequest request);

    Task<LessLivestockDailyResponseDto> SaveEntry(LessLivestockDailySaveRequest request);

    Task<PagedList<LessLivestockDailyHeaderDto>> GetHeaders(LessLivestockDailyHeadersRequest request);
}
