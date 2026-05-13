using authentication_engine.Shared;

namespace authentication_engine.Features.Ranks.Interfaces
{
    public interface IRankRepository
    {
        Task<PagedList<Rank>> GetPagedData(PaginationDto dto);

        Task<Rank> Create(Rank rank);

        Task<Rank> GetById(Guid id);

        Task<Rank> Update(Guid id, Rank rank);

        Task<bool> Delete(Guid id);
    }
}
