using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.GroundCounts.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.GroundCounts;

public class GroundCountService(IGroundCountRepository repository, IFileService fileService, IMapper mapper): IGroundCountService
{
    private readonly IGroundCountRepository _groundCountRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<GroundCountResponseDto>> GetGroundCounts(GroundCountPaginationDto dto)
    {
        var pagedData = await _groundCountRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<GroundCountResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<GroundCountResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetGroundCountDto> CreateGroundCount(GroundCountRequestDto dto)
    {
        var groundCountId = await _groundCountRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(groundCountId))
            throw new ArgumentNullException("Failure to add ground count data");
        
        return await _groundCountRepository.GetById(Guid.Parse(groundCountId));
    }

    public async Task<GetGroundCountDto> GetGroundCountById(Guid id)
    {
        var result = await _groundCountRepository.GetById(id);

        foreach (var groundCount in result.GroundCountSightings)
        {
            groundCount.File = await _fileService.GetSingleFileByEntityData(
                groundCount.Id, AppEntities.GroundCountSightingEntity
            );
        }

        return result;
    }

    public async Task<GetGroundCountDto> UpdateGroundCount(Guid id, GroundCountRequestDto dto)
    {
        var groundCountId =  await _groundCountRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(groundCountId))
            throw new ArgumentNullException("Failure to update ground count data");
        
        return await _groundCountRepository.GetById(Guid.Parse(groundCountId));
    }

    public async Task<bool> DeleteGroundCount(Guid id)
    {
        return await _groundCountRepository.Delete(id);
    }
}