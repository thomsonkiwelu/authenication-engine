using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQualities.Interfaces;

public interface IWaterQualityRepository
{
    Task<PagedList<WaterQuality>> GetPagedData(WaterQualityPaginationDto dto);

    Task<WaterQuality> Create(WaterQuality waterQuality);

    Task<WaterQuality> GetById(Guid id);

    Task<WaterQuality> Update(Guid id,  WaterQuality waterQuality);

    Task<bool> Delete(Guid id);
}