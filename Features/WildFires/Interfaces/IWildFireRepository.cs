using conservation_backend.Shared;

namespace conservation_backend.Features.WildFires.Interfaces;

public interface IWildFireRepository
{
    Task<PagedList<WildFire>> GetPagedData(WildFirePaginationDto dto);

    Task<string> Create(WildFireRequestDto dto);

    Task<GetWildFireDto> GetById(Guid id);

    Task<string> Update(Guid id, WildFireRequestDto dto);

    Task<bool> Delete(Guid id);
}