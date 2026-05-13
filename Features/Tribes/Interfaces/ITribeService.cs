using conservation_backend.Shared;

namespace conservation_backend.Features.Tribes.Interfaces;

public interface ITribeService
{
    Task<PagedList<TribeResponseDto>> GetAllTribesData(PaginationDto dto);

    Task<TribeDto> CreateTribe(TribeRequestDto dto);

    Task<TribeDto> GetTribeById(Guid id);

    Task<TribeDto> UpdateTribe(Guid id, TribeRequestDto dto);

    Task<bool> DeleteTribe(Guid id);
}