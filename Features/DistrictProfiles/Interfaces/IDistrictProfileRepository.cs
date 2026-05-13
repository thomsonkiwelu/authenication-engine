using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictProfiles.Interfaces;

public interface IDistrictProfileRepository
{
    Task<PagedList<DistrictProfile>> GetPagedData(DistrictProfilePaginationDto dto);
    
    Task<string> Create(DistrictProfileRequestDto dto);

    Task<GetDistrictProfileDto> GetById(Guid id);

    Task<string> Update(Guid id, DistrictProfileRequestDto dto);

    Task<bool> Delete(Guid id);

    Task<GetFullDistrictProfileDto> GetFullDetails(Guid id);
}