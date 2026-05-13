using conservation_backend.Features.DeathTurtles.Interfaces;
using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.DeathTurtles;

public class DeathTurtleService(IDeathTurtleRepository repository, IFileService fileService, IMapper mapper): IDeathTurtleService
{
    private readonly IDeathTurtleRepository _deathTurtleRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<DeathTurtleResponseDto>> GetAllDeathTurtles(DeathTurtlePaginationDto dto)
    {
        var pagedData = await _deathTurtleRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<DeathTurtleResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<DeathTurtleResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<DeathTurtleDto> CreateDeathTurtle(DeathTurtleRequestDto dto)
    {
        var deathTurtle = _mapper.Map<DeathTurtle>(dto);
        var result = await _deathTurtleRepository.Create(deathTurtle);
        
        var responseDto = _mapper.Map<DeathTurtleDto>(result);
        // Update Image
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.DeathTurtleEntity, result.Id);
        
        return responseDto;
    }

    public async Task<DeathTurtleDto> GetDeathTurtleById(Guid id)
    {
        var deathTurtle = await _deathTurtleRepository.GetById(id);
        var result = _mapper.Map<DeathTurtleDto>(deathTurtle);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.DeathTurtleEntity
        );
        
        return result;
    }

    public async Task<DeathTurtleDto> UpdateDeathTurtle(Guid id, DeathTurtleRequestDto dto)
    {
        var deathTurtle = _mapper.Map<DeathTurtle>(dto);
        var updated = await _deathTurtleRepository.Update(id, deathTurtle);

        var responseDto = _mapper.Map<DeathTurtleDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteDeathTurtle(Guid id)
    {
        return await _deathTurtleRepository.Delete(id);
    }
}