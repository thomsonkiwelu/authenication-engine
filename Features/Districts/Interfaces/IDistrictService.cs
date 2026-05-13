using conservation_backend.Shared;

namespace conservation_backend.Features.Districts.Interfaces;

public interface IDistrictService
{
    Task<PagedList<DistrictResponseDto>> GetAllDistricts(DistrictPaginationDto dto);

    Task<DistrictDto> CreateDistrict(DistrictRequest dto);

    Task<DistrictDto> GetDistrictById(Guid id);

    Task<DistrictDto> UpdateDistrict(Guid id, DistrictRequest dto);

    Task<bool> DeleteDistrict(Guid id);
}