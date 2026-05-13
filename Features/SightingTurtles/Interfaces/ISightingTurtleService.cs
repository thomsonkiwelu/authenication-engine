using conservation_backend.Shared;

namespace conservation_backend.Features.SightingTurtles.Interfaces;

public interface ISightingTurtleService
{
    Task<PagedList<SightingTurtleResponseDto>> GetSightingTurtles(SightingTurtlePaginationDto dto);
    
    Task<GetSightingTurtleDto> CreateSightingTurtle(SightingTurtleRequestDto dto);
    
    Task<GetSightingTurtleDto> GetSightingTurtleById(Guid id);

    Task<GetSightingTurtleDto> UpdateSightingTurtle(Guid id , SightingTurtleRequestDto dto);

    Task<bool> DeleteSightingTurtle(Guid id);
}