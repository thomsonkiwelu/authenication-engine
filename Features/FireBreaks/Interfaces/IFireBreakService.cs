using conservation_backend.Shared;

namespace conservation_backend.Features.FireBreaks.Interfaces;

public interface IFireBreakService
{
    Task<PagedList<FireBreakResponseDto>> GetFireBreaks(FireBreakPaginationDto dto);
    
    Task<GetFireBreakDto> CreateFireBreak(FireBreakRequestDto dto);
    
    Task<GetFireBreakDto> GetFireBreakById(Guid id);

    Task<GetFireBreakDto> UpdateFireBreak(Guid id , FireBreakRequestDto dto);

    Task<bool> DeleteFireBreak(Guid id);
}