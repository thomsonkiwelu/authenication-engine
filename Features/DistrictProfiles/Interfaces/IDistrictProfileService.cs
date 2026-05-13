using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictProfiles.Interfaces;

public interface IDistrictProfileService
{
    Task<PagedList<DistrictProfileResponseDto>> GetDistrictProfiles(DistrictProfilePaginationDto dto);
    
    Task<GetDistrictProfileDto> CreateDistrictProfile(DistrictProfileRequestDto dto);
    
    Task<GetDistrictProfileDto> GetDistrictProfileById(Guid id);

    Task<GetDistrictProfileDto> UpdateDistrictProfile(Guid id , DistrictProfileRequestDto dto);

    Task<bool> DeleteDistrictProfile(Guid id);

    Task<GetFullDistrictProfileDto> GetFullDistrictProfileById(Guid id);
}