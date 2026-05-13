using conservation_backend.Shared;

namespace conservation_backend.Features.FireBreaks.Interfaces;

public interface IFireBreakRepository
{
    Task<PagedList<FireBreak>> GetPagedData(FireBreakPaginationDto dto);

    Task<string> Create(FireBreakRequestDto dto);

    Task<GetFireBreakDto> GetById(Guid id);

    Task<string> Update(Guid id, FireBreakRequestDto dto);

    Task<bool> Delete(Guid id);
}