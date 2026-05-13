using conservation_backend.Shared;

namespace conservation_backend.Features.VillageProfiles.Interfaces;

public interface IVillageProfileService
{
    Task<PagedList<VillageProfileResponseDto>> GetAllVillageProfilesData(VillageProfilePaginationDto dto);

    Task<VillageProfileDto> CreateVillageProfile(VillageProfileRequestDto dto);

    Task<VillageProfileDto> GetVillageProfileById(Guid id);

    Task<VillageProfileDto> UpdateVillageProfile(Guid id, VillageProfileRequestDto dto);

    Task<bool> DeleteVillageProfile(Guid id);
    
    Task<GetVillageProfileDto> GetFullVillageProfileById(Guid id);
}