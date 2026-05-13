using conservation_backend.Shared;

namespace conservation_backend.Features.Species.Interfaces
{
    public interface ISpeciesRepository
    {
        Task<PagedList<Species>> GetPagedData(SpeciesPaginationDto dto);

        Task<Species> Create(Species species);

        Task<Species> GetById(Guid id);

        Task<Species> Update(Guid id, Species species);

        Task<bool> Delete(Guid id);
    }
}
