using conservation_backend.Shared;

namespace conservation_backend.Features.Parks.Interfaces;

public interface IParkService
{
    Task<PagedList<ParkResponseDto>> GetAllParksData(ParkPaginationDto dto);

    Task<ParkDto> CreatePark(ParkRequest dto);

    Task<ParkDto> GetParkById(Guid id);

    Task<ParkDto> UpdatePark(Guid id, ParkRequest dto);

    Task<bool> DeletePark(Guid id);
}