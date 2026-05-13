using conservation_backend.Shared;

namespace conservation_backend.Features.Species.Interfaces
{
    public interface ISpeciesService
    {
        Task<PagedList<SpeciesResponseDto>> GetAllSpeciesData(SpeciesPaginationDto dto);

        Task<SpeciesDto> CreateSpecies(SpeciesRequest dto);

        Task<SpeciesDto> GetSpeciesById(Guid id);

        Task<SpeciesDto> UpdateSpecies(Guid id, SpeciesRequest dto);

        Task<bool> DeleteSpecies(Guid id);
    }
}
