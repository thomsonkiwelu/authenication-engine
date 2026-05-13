using conservation_backend.Shared;

namespace conservation_backend.Features.FireSeminars.Interfaces;

public interface IFireSeminarService
{
    Task<PagedList<FireSeminarResponseDto>> GetFireSeminars(FireSeminarPaginationDto dto);
    
    Task<GetFireSeminarDto> CreateFireSeminar(FireSeminarRequestDto dto);
    
    Task<GetFireSeminarDto> GetFireSeminarById(Guid id);

    Task<GetFireSeminarDto> UpdateFireSeminar(Guid id , FireSeminarRequestDto dto);

    Task<bool> DeleteFireSeminar(Guid id);
}