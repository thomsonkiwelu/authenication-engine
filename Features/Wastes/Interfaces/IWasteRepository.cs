using conservation_backend.Shared;

namespace conservation_backend.Features.Wastes.Interfaces
{
    public interface IWasteRepository
    {
        Task<PagedList<WasteResponseDto>> GetPagedData(WastePaginationDto dto);

        Task<string> Create(WasteRequestDto dto);

        Task<GetWasteDto> GetById(Guid id);

        Task<string> Update(Guid id, WasteRequestDto dto);

        Task<bool> Delete(Guid id);
    }
}
