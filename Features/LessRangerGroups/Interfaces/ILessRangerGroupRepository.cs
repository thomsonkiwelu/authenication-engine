using authentication_engine.Shared;

namespace authentication_engine.Features.LessRangerGroups.Interfaces;

public interface ILessRangerGroupRepository
{
    Task<PagedList<LessRangerGroup>> GetPagedData(LessRangerGroupPaginationDto dto);

    Task<LessRangerGroup> Create(LessRangerGroup group);

    Task<LessRangerGroup> GetById(Guid id);

    Task<LessRangerGroup> Update(Guid id, LessRangerGroup group);

    Task<bool> Delete(Guid id);
}
