using authentication_engine.Shared;

namespace authentication_engine.Features.Staffs.Interfaces;

public interface IStaffService
{
    Task<PagedList<StaffResponseDto>> GetAllStaffsData(StaffPaginationDto dto);

    Task<StaffDto> CreateStaff(StaffRequest dto);

    Task<StaffDto> GetStaffById(Guid id);

    Task<StaffDto> UpdateStaff(Guid id, StaffRequest dto);

    Task<bool> DeleteStaff(Guid id);
    
    Task<OrganizationContextDto> GetStaffsWithOrganizationContext(Guid officeId);
}