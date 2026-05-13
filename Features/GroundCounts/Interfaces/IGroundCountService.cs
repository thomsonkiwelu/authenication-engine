using conservation_backend.Shared;

namespace conservation_backend.Features.GroundCounts.Interfaces;

public interface IGroundCountService
{
    Task<PagedList<GroundCountResponseDto>> GetGroundCounts(GroundCountPaginationDto dto);
    
    Task<GetGroundCountDto> CreateGroundCount(GroundCountRequestDto dto);
    
    Task<GetGroundCountDto> GetGroundCountById(Guid id);

    Task<GetGroundCountDto> UpdateGroundCount(Guid id , GroundCountRequestDto dto);

    Task<bool> DeleteGroundCount(Guid id);
}