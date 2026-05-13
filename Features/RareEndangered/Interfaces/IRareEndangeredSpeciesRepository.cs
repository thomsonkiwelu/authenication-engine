using conservation_backend.Shared;

namespace conservation_backend.Features.RareEndangered.Interfaces;

public interface IRareEndangeredSpeciesRepository
{
    Task<PagedList<RareEndangeredSpeciesResponseDto>> GetPagedData(RareEndangeredSpeciesPaginationDto dto);
    
    Task<string> Create(RareEndangeredSpeciesRequestDto dto);

    Task<GetRareEndangeredSpeciesDto> GetById(Guid id);

    Task<string> Update(Guid id, RareEndangeredSpeciesRequestDto dto);

    Task<bool> Delete(Guid id);
}