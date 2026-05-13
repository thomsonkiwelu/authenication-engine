using conservation_backend.Shared;

namespace conservation_backend.Features.FireSeminars.Interfaces;

public interface IFireSeminarRepository
{
    Task<PagedList<FireSeminar>> GetPagedData(FireSeminarPaginationDto dto);

    Task<string> Create(FireSeminarRequestDto dto);

    Task<GetFireSeminarDto> GetById(Guid id);

    Task<string> Update(Guid id, FireSeminarRequestDto dto);

    Task<bool> Delete(Guid id);
}