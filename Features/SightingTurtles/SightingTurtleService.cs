using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.SightingTurtles.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.SightingTurtles;

public class SightingTurtleService(ISightingTurtleRepository repository, IFileService fileService, IMapper mapper): ISightingTurtleService
{
    private readonly ISightingTurtleRepository _sightingTurtleRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<SightingTurtleResponseDto>> GetSightingTurtles(SightingTurtlePaginationDto dto)
    {
        var pagedData = await _sightingTurtleRepository.GetPagedData(dto);
        
        return new PagedList<SightingTurtleResponseDto>(
            items: pagedData.Data,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<GetSightingTurtleDto> CreateSightingTurtle(SightingTurtleRequestDto dto)
    {
        var sightingTurtleId = await _sightingTurtleRepository.Create(dto);
        
        if (string.IsNullOrWhiteSpace(sightingTurtleId))
            throw new ArgumentNullException("Failure to create sighting turtle data");
        
        // Update Image data
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.SightingTurtleEntity, Guid.Parse(sightingTurtleId));

        return await _sightingTurtleRepository.GetById(Guid.Parse(sightingTurtleId));
    }

    public async Task<GetSightingTurtleDto> GetSightingTurtleById(Guid id)
    {
        var result = await _sightingTurtleRepository.GetById(id);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            id, AppEntities.SightingTurtleEntity
        );
        
        return result;
    }

    public async Task<GetSightingTurtleDto> UpdateSightingTurtle(Guid id, SightingTurtleRequestDto dto)
    {
        var sightingTurtleId = await _sightingTurtleRepository.Update(id, dto);
        
        if (string.IsNullOrWhiteSpace(sightingTurtleId))
            throw new ArgumentNullException("Failure to update sighting turtle data");
        
        return await _sightingTurtleRepository.GetById(Guid.Parse(sightingTurtleId));
    }

    public async Task<bool> DeleteSightingTurtle(Guid id)
    {
        return await _sightingTurtleRepository.Delete(id);
    }
}