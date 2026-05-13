using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.FireBreaks.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.FireBreaks;

public class FireBreakService(IFireBreakRepository repository, IFileService fileService, IMapper mapper): IFireBreakService
{
    private readonly IFireBreakRepository _fireBreakRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<FireBreakResponseDto>> GetFireBreaks(FireBreakPaginationDto dto)
    {
        var pagedData = await _fireBreakRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<FireBreakResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<FireBreakResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetFireBreakDto> CreateFireBreak(FireBreakRequestDto dto)
    {
        var fireBreakId = await _fireBreakRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(fireBreakId))
            throw new ArgumentNullException("Failure to create firebreak data");
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.FireBreakEntity, Guid.Parse(fireBreakId));

        return await _fireBreakRepository.GetById(Guid.Parse(fireBreakId));
    }

    public async Task<GetFireBreakDto> GetFireBreakById(Guid id)
    {
        var result = await _fireBreakRepository.GetById(id);
        
        result.File = await _fileService.GetFileByEntityData(
            id, AppEntities.FireBreakEntity
        );
        
        return result;
    }

    public async Task<GetFireBreakDto> UpdateFireBreak(Guid id, FireBreakRequestDto dto)
    {
        var fireBreakId = await _fireBreakRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(fireBreakId))
            throw new ArgumentNullException("Failure to update firebreak data");
        
        return await _fireBreakRepository.GetById(Guid.Parse(fireBreakId));
    }

    public async Task<bool> DeleteFireBreak(Guid id)
    {
        return await _fireBreakRepository.Delete(id);
    }
}