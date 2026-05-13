using conservation_backend.Shared;

namespace conservation_backend.Features.WaterBodies.Interfaces;

public interface IWaterBodyService
{
    Task<PagedList<WaterBodyResponseDto>> GetAllWaterBodiesData(WaterBodyPaginationDto dto);

    Task<WaterBodyDto> CreateWaterBody(WaterBodyRequest dto);

    Task<WaterBodyDto> GetWaterBodyById(Guid id);

    Task<WaterBodyDto> UpdateWaterBody(Guid id, WaterBodyRequest dto);

    Task<bool> DeleteWaterBody(Guid id);
}