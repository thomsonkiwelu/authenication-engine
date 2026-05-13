using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQuantities.Interfaces;

public interface IWaterQuantityRepository
{
    Task<PagedList<WaterQuantity>> GetPagedData(WaterQuantityPaginationDto dto);

    Task<WaterQuantity> Create(WaterQuantity waterQuantity);

    Task<WaterQuantity> GetById(Guid id);

    Task<WaterQuantity> Update(Guid id,  WaterQuantity waterQuantity);

    Task<bool> Delete(Guid id);
}