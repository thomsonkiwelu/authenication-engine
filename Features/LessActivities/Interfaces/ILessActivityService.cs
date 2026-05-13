using conservation_backend.Shared;

namespace conservation_backend.Features.LessActivities.Interfaces;

public interface ILessActivityService
{
    Task<PagedList<LessActivityResponseDto>> GetAllLessActivitiesData(LessActivityPaginationDto dto);

    Task<LessActivityDto> CreateLessActivity(LessActivityRequest dto);

    Task<LessActivityDto> GetLessActivityById(Guid id);

    Task<LessActivityDto> UpdateLessActivity(Guid id, LessActivityRequest dto);

    Task<bool> DeleteLessActivity(Guid id);
}