using authentication_engine.Features.LessRangerGroups.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.LessRangerGroups;

public class LessRangerGroupService(ILessRangerGroupRepository repository, IMapper mapper) : ILessRangerGroupService
{
    private readonly ILessRangerGroupRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<LessRangerGroupResponseDto>> GetAllGroupsData(LessRangerGroupPaginationDto dto)
    {
        var pagedData = await _repository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<LessRangerGroupResponseDto>>(pagedData.Data)
            .Select((item, index) => item with { RowNumber = startIndex + index })
            .ToList();

        return new PagedList<LessRangerGroupResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<LessRangerGroupDto> CreateGroup(LessRangerGroupRequest dto)
    {
        var group = _mapper.Map<LessRangerGroup>(dto);
        var created = await _repository.Create(group);

        return _mapper.Map<LessRangerGroupDto>(created);
    }

    public async Task<LessRangerGroupDto> GetGroupById(Guid id)
    {
        var group = await _repository.GetById(id);
        return _mapper.Map<LessRangerGroupDto>(group);
    }

    public async Task<LessRangerGroupDto> UpdateGroup(Guid id, LessRangerGroupRequest dto)
    {
        var group = _mapper.Map<LessRangerGroup>(dto);
        var updated = await _repository.Update(id, group);

        return _mapper.Map<LessRangerGroupDto>(updated);
    }

    public async Task<bool> DeleteGroup(Guid id)
    {
        return await _repository.Delete(id);
    }
}
