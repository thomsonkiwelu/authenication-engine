using conservation_backend.Shared;

namespace conservation_backend.Features.WildFires.Interfaces;

public interface IWildFireService
{
    Task<PagedList<WildFireResponseDto>> GetWildFires(WildFirePaginationDto dto);
    
    Task<GetWildFireDto> CreateWildFire(WildFireRequestDto dto);
    
    Task<GetWildFireDto> GetWildFireById(Guid id);

    Task<GetWildFireDto> UpdateWildFire(Guid id , WildFireRequestDto dto);

    Task<bool> DeleteWildFire(Guid id);
}