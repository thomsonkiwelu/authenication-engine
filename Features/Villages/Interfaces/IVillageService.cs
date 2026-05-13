using conservation_backend.Shared;

namespace conservation_backend.Features.Villages.Interfaces;

public interface IVillageService
{
    Task<PagedList<VillageResponseDto>> GetAllVillages(VillagePaginationDto dto);

    Task<VillageDto> CreateVillage(VillageRequest dto);

    Task<VillageDto> GetVillageById(Guid id);

    Task<VillageDto> UpdateVillage(Guid id, VillageRequest dto);

    Task<bool> DeleteVillage(Guid id);
}