using conservation_backend.Shared;

namespace conservation_backend.Features.LineTransects.Interfaces;

public interface ILineTransectRepository
{
    Task<PagedList<LineTransectResponseDto>> GetPagedData(LineTransectPaginationDto dto);
    
    Task<string> Create(LineTransectRequestDto dto);

    Task<GetLineTransectDto> GetById(Guid id);

    Task<string> Update(Guid id, LineTransectRequestDto dto);

    Task<bool> Delete(Guid id);
}