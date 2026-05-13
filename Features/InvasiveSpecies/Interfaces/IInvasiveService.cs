using conservation_backend.Shared;

namespace conservation_backend.Features.InvasiveSpecies.Interfaces;

public interface IInvasiveService
{
    Task<PagedList<InvasiveResponseDto>> GetPagedInvasiveSpecies(InvasivePaginationDto dto);
    
    Task<GetInvasiveSpeciesDto> CreateInvasiveSpecies(InvasiveRequestDto dto);
    
    Task<GetInvasiveSpeciesDto> GetInvasiveSpeciesById(Guid id);

    Task<GetInvasiveSpeciesDto> UpdateInvasiveSpecies(Guid id , InvasiveRequestDto dto);

    Task<bool> DeleteInvasiveSpecies(Guid id);
}