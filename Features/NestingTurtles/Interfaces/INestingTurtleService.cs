using conservation_backend.Shared;

namespace conservation_backend.Features.NestingTurtles.Interfaces;

public interface INestingTurtleService
{
    Task<PagedList<NestingTurtleResponseDto>> GetAllNestingTurtles(NestingTurtlePaginationDto dto);

    Task<NestingTurtleDto> CreateNestingTurtle(NestingTurtleRequestDto dto);

    Task<NestingTurtleDto> GetNestingTurtleById(Guid id);

    Task<NestingTurtleDto> UpdateNestingTurtle(Guid id, NestingTurtleRequestDto dto);

    Task<bool> DeleteNestingTurtle(Guid id);
}