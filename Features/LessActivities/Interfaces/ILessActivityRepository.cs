using conservation_backend.Shared;

namespace conservation_backend.Features.LessActivities.Interfaces;

public interface ILessActivityRepository
{
    Task<PagedList<LessActivity>> GetPagedData(LessActivityPaginationDto dto);

    Task<LessActivity> Create(LessActivity lessActivity);

    Task<LessActivity> GetById(Guid id);

    Task<LessActivity> Update(Guid id, LessActivity lessActivity);

    Task<bool> Delete(Guid id);
}