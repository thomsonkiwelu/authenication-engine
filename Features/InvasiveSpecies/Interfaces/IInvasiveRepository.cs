using conservation_backend.Config;
using conservation_backend.Shared;

namespace conservation_backend.Features.InvasiveSpecies.Interfaces;

public interface IInvasiveRepository
{
    Task<PagedList<InvasiveResponseDto>> GetPagedData(InvasivePaginationDto dto);

    Task<string> Create(InvasiveRequestDto dto);

    Task<GetInvasiveSpeciesDto> GetById(Guid id);

    Task<string> Update(Guid id, InvasiveRequestDto dto);

    Task<bool> Delete(Guid id);
}