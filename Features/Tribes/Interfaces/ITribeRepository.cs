using conservation_backend.Shared;

namespace conservation_backend.Features.Tribes.Interfaces;

public interface ITribeRepository
{
    Task<PagedList<Tribe>> GetPagedData(PaginationDto dto);

    Task<Tribe> Create(Tribe tribe);

    Task<Tribe> GetById(Guid id);

    Task<Tribe> Update(Guid id, Tribe tribe);

    Task<bool> Delete(Guid id);
}