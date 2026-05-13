using conservation_backend.Shared;

namespace conservation_backend.Features.RareEndangered.Interfaces;

public interface IRareEndangeredSpeciesService
{
    Task<PagedList<RareEndangeredSpeciesResponseDto>> GetPagedRareEndangeredSpecies(RareEndangeredSpeciesPaginationDto dto);
    
    Task<GetRareEndangeredSpeciesDto> CreateRareEndangeredSpecies(RareEndangeredSpeciesRequestDto dto);
    
    Task<GetRareEndangeredSpeciesDto> GetRareEndangeredSpeciesById(Guid id);

    Task<GetRareEndangeredSpeciesDto> UpdateRareEndangeredSpecies(Guid id , RareEndangeredSpeciesRequestDto dto);

    Task<bool> DeleteRareEndangeredSpecies(Guid id);
}