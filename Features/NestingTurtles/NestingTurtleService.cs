using conservation_backend.Features.Files.Interfaces;
using conservation_backend.Features.NestingTurtles.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;

namespace conservation_backend.Features.NestingTurtles;

public class NestingTurtleService(INestingTurtleRepository repository, IFileService fileService, IMapper mapper): INestingTurtleService
{
    private readonly INestingTurtleRepository _nestingTurtleRepository = repository;
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;
    
    public async Task<PagedList<NestingTurtleResponseDto>> GetAllNestingTurtles(NestingTurtlePaginationDto dto)
    {
        var pagedData = await _nestingTurtleRepository.GetPagedData(dto);

        var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

        var dtoList = _mapper.Map<List<NestingTurtleResponseDto>>(pagedData.Data)
            .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

        return new PagedList<NestingTurtleResponseDto>(
            items: dtoList,
            page: pagedData.Page,
            pageSize: pagedData.PageSize,
            totalCount: pagedData.TotalCount
        );
    }

    public async Task<NestingTurtleDto> CreateNestingTurtle(NestingTurtleRequestDto dto)
    {
        var nestingTurtle = _mapper.Map<NestingTurtle>(dto);
        var result = await _nestingTurtleRepository.Create(nestingTurtle);
        
        var responseDto = _mapper.Map<NestingTurtleDto>(result);
        // Update Image
        if (!string.IsNullOrWhiteSpace(dto.ImageId))
            await _fileService.UpdateUploadedFile(dto.ImageId, AppEntities.NestingTurtleEntity, result.Id);
        
        return responseDto;
    }

    public async Task<NestingTurtleDto> GetNestingTurtleById(Guid id)
    {
        var nestingTurtle = await _nestingTurtleRepository.GetById(id);
        var result = _mapper.Map<NestingTurtleDto>(nestingTurtle);
        
        result.File = await _fileService.GetSingleFileByEntityData(
            result.Id, AppEntities.NestingTurtleEntity
        );
        
        return result;
    }

    public async Task<NestingTurtleDto> UpdateNestingTurtle(Guid id, NestingTurtleRequestDto dto)
    {
        var nestingTurtle = _mapper.Map<NestingTurtle>(dto);
        var updated = await _nestingTurtleRepository.Update(id, nestingTurtle);

        var responseDto = _mapper.Map<NestingTurtleDto>(updated);
        return responseDto;
    }

    public async Task<bool> DeleteNestingTurtle(Guid id)
    {
        return await _nestingTurtleRepository.Delete(id);
    }
}