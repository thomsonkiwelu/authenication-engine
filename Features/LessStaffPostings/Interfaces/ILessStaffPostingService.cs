using authentication_engine.Shared;

namespace authentication_engine.Features.LessStaffPostings.Interfaces;

public interface ILessStaffPostingService
{
    Task<PagedList<LessStaffPostingDto>> GetPostings(LessStaffPostingPaginationDto dto);

    Task<LessStaffPostingDto> Assign(LessStaffPostingAssignRequest request);

    Task<List<LessStaffPostingDto>> BulkAssign(LessStaffPostingBulkAssignRequest request);

    Task<bool> Unassign(LessStaffPostingUnassignRequest request);

    Task<List<StaffOptionDto>> GetParkStaffOptions(string parkId);

    Task<List<StaffOptionDto>> GetOfficeStaffOptions(string officeId);
}
