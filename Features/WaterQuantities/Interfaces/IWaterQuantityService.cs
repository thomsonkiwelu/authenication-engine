using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQuantities.Interfaces;

public interface IWaterQuantityService
{
    Task<PagedList<WaterQuantityResponseDto>> GetAllWaterQuantitiesData(WaterQuantityPaginationDto dto);

    Task<WaterQuantityDto> CreateWaterQuantity(WaterQuantityRequestDto dto);

    Task<WaterQuantityDto> GetWaterQuantityById(Guid id);

    Task<WaterQuantityDto> UpdateWaterQuantity(Guid id, WaterQuantityRequestDto dto);

    Task<bool> DeleteWaterQuantity(Guid id);
}