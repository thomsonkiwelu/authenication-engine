using conservation_backend.Features.LessActivities.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.LessActivities;

public class LessActivityService(ILessActivityRepository repository, IMapper mapper): ILessActivityService
{
    private readonly ILessActivityRepository _lessActivityRepository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<LessActivityResponseDto>> GetAllLessActivitiesData(LessActivityPaginationDto dto)
    {
        var pagedData = await _lessActivityRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<LessActivityResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<LessActivityResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<LessActivityDto> CreateLessActivity(LessActivityRequest dto)
    {
        var lessActivity = _mapper.Map<LessActivity>(dto);
        var result = await _lessActivityRepository.Create(lessActivity);
        
        var responseDto = _mapper.Map<LessActivityDto>(result);
        return responseDto;
    }

    public async Task<LessActivityDto> GetLessActivityById(Guid id)
    {
        var lessActivity = await _lessActivityRepository.GetById(id);
        
        var result = _mapper.Map<LessActivityDto>(lessActivity);
        return result;
    }

    public async Task<LessActivityDto> UpdateLessActivity(Guid id, LessActivityRequest dto)
    {
        var lessActivity = _mapper.Map<LessActivity>(dto);
        var updated = await _lessActivityRepository.Update(id, lessActivity);

        var responseDto = _mapper.Map<LessActivityDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteLessActivity(Guid id)
    {
        return await _lessActivityRepository.Delete(id);
    }
}