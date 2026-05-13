using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQualities.Interfaces;

public interface IWaterQualityService
{
    Task<PagedList<WaterQualityResponseDto>> GetAllWaterQualitiesData(WaterQualityPaginationDto dto);

    Task<WaterQualityDto> CreateWaterQuality(WaterQualityRequestDto dto);

    Task<WaterQualityDto> GetWaterQualityById(Guid id);

    Task<WaterQualityDto> UpdateWaterQuality(Guid id, WaterQualityRequestDto dto);

    Task<bool> DeleteWaterQuality(Guid id);
}