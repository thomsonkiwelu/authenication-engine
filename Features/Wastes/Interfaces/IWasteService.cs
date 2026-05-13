using conservation_backend.Shared;

namespace conservation_backend.Features.Wastes.Interfaces
{
    public interface IWasteService
    {
        Task<PagedList<WasteResponseDto>> GetAllWastesData(WastePaginationDto dto);
    
        Task<GetWasteDto> CreateWaste(WasteRequestDto dto);
        
        Task<GetWasteDto> GetWasteById(Guid id);

        Task<GetWasteDto> UpdateWaste(Guid id, WasteRequestDto dto);

        Task<bool> DeleteWaste(Guid id);
    }
}
