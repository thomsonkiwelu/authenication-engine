using conservation_backend.Shared;

namespace conservation_backend.Features.SightingTurtles.Interfaces;

public interface ISightingTurtleRepository
{
    Task<PagedList<SightingTurtleResponseDto>> GetPagedData(SightingTurtlePaginationDto dto);

    Task<string> Create(SightingTurtleRequestDto dto);

    Task<GetSightingTurtleDto> GetById(Guid id);

    Task<string> Update(Guid id, SightingTurtleRequestDto dto);

    Task<bool> Delete(Guid id);
}