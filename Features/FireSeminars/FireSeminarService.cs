using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.FireSeminars.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.FireSeminars;

public class FireSeminarService(IFireSeminarRepository repository, IFileService fileService, IMapper mapper): IFireSeminarService
{
    private readonly IFireSeminarRepository _fireSeminarRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<FireSeminarResponseDto>> GetFireSeminars(FireSeminarPaginationDto dto)
    {
        var pagedData = await _fireSeminarRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<FireSeminarResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<FireSeminarResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetFireSeminarDto> CreateFireSeminar(FireSeminarRequestDto dto)
    {
        var fireSeminarId = await _fireSeminarRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(fireSeminarId))
            throw new ArgumentNullException("Failure to create fire seminar data");
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.FireSeminarEntity, Guid.Parse(fireSeminarId));

        return await _fireSeminarRepository.GetById(Guid.Parse(fireSeminarId));
    }

    public async Task<GetFireSeminarDto> GetFireSeminarById(Guid id)
    {
        var result = await _fireSeminarRepository.GetById(id);
        
        result.File = await _fileService.GetFileByEntityData(
            id, AppEntities.FireSeminarEntity
        );
        
        return result;
    }

    public async Task<GetFireSeminarDto> UpdateFireSeminar(Guid id, FireSeminarRequestDto dto)
    {
        var fireSeminarId = await _fireSeminarRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(fireSeminarId))
            throw new ArgumentNullException("Failure to update seminar data");
        
        return await _fireSeminarRepository.GetById(Guid.Parse(fireSeminarId));
    }

    public async Task<bool> DeleteFireSeminar(Guid id)
    {
        return await _fireSeminarRepository.Delete(id);
    }
}