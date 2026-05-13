using conservation_backend.Shared;

namespace conservation_backend.Features.LineTransects.Interfaces;

public interface ILineTransectService
{
    Task<PagedList<LineTransectResponseDto>> GetPagedLineTransects(LineTransectPaginationDto dto);
    
    Task<GetLineTransectDto> CreateLineTransect(LineTransectRequestDto dto);
    
    Task<GetLineTransectDto> GetLineTransectById(Guid id);

    Task<GetLineTransectDto> UpdateLineTransect(Guid id , LineTransectRequestDto dto);

    Task<bool> DeleteLineTransect(Guid id);
}