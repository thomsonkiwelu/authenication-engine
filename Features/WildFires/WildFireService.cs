
using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.WildFires.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.WildFires;

public class WildFireService(IWildFireRepository repository, IFileService fileService, IMapper mapper): IWildFireService
{
    private readonly IWildFireRepository _wildFireRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<WildFireResponseDto>> GetWildFires(WildFirePaginationDto dto)
    {   
        var pagedData = await _wildFireRepository.GetPagedData(dto);
        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<WildFireResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<WildFireResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetWildFireDto> CreateWildFire(WildFireRequestDto dto)
    {
        var wildFireId = await _wildFireRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(wildFireId))
            throw new ArgumentNullException("Failure to create wild fire data");
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.WildFireEntity, Guid.Parse(wildFireId));

        return await _wildFireRepository.GetById(Guid.Parse(wildFireId));
    }

    public async Task<GetWildFireDto> GetWildFireById(Guid id)
    {
        var result = await _wildFireRepository.GetById(id);
        
        result.File = await _fileService.GetFileByEntityData(
            id, AppEntities.WildFireEntity
        );
        
        return result;
    }

    public async Task<GetWildFireDto> UpdateWildFire(Guid id, WildFireRequestDto dto)
    {
        var wildFireId = await _wildFireRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(wildFireId))
            throw new ArgumentNullException("Failure to update wildfire data");
        
        return await _wildFireRepository.GetById(Guid.Parse(wildFireId));
    }

    public async Task<bool> DeleteWildFire(Guid id)
    {
        return await _wildFireRepository.Delete(id);
    }
}