using authentication_engine.Shared;

namespace authentication_engine.Features.Staffs.Interfaces;

public interface IStaffRepository
{
    Task<PagedList<Staff>> GetPagedData(StaffPaginationDto dto);

    Task<Staff> Create(Staff staff);

    Task<Staff> GetById(Guid id);

    Task<Staff> Update(Guid id, Staff staff);

    Task<bool> Delete(Guid id);
    
    Task<OrganizationContextDto> GetStaffsWithOrganizationContext(Guid officeId);
}