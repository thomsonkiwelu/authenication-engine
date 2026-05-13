using conservation_backend.Shared;

namespace conservation_backend.Features.GroundCounts.Interfaces;

public interface IGroundCountRepository
{
    Task<PagedList<GroundCount>> GetPagedData(GroundCountPaginationDto dto);

    Task<string> Create(GroundCountRequestDto dto);

    Task<GetGroundCountDto> GetById(Guid id);

    Task<string> Update(Guid id, GroundCountRequestDto dto);

    Task<bool> Delete(Guid id);
}