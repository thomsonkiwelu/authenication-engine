using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerGroups.Interfaces;

public interface ILessRangerGroupService
{
    Task<PagedList<LessRangerGroupResponseDto>> GetAllGroupsData(LessRangerGroupPaginationDto dto);

    Task<LessRangerGroupDto> CreateGroup(LessRangerGroupRequest dto);

    Task<LessRangerGroupDto> GetGroupById(Guid id);

    Task<LessRangerGroupDto> UpdateGroup(Guid id, LessRangerGroupRequest dto);

    Task<bool> DeleteGroup(Guid id);
}
