using conservation_backend.Shared;

namespace conservation_backend.Features.WaterBodies.Interfaces;

public interface IWaterBodyRepository
{
    Task<PagedList<WaterBody>> GetPagedData(WaterBodyPaginationDto dto);

    Task< WaterBody> Create( WaterBody waterBody);

    Task< WaterBody> GetById(Guid id);

    Task< WaterBody> Update(Guid id,  WaterBody waterBody);

    Task<bool> Delete(Guid id);
}