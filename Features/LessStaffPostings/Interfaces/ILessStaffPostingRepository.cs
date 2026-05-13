using authentication_engine.Shared;

namespace authentication_engine.Features.LessStaffPostings.Interfaces;

public interface ILessStaffPostingRepository
{
    Task<PagedList<LessStaffPosting>> GetPagedData(LessStaffPostingPaginationDto dto);

    Task<LessStaffPosting?> GetActivePostingByStaffId(Guid staffId);

    Task CloseActivePostingByStaffId(Guid staffId, string? remarks);

    Task<LessStaffPosting> Create(LessStaffPosting posting);

    Task<bool> ClosePosting(Guid id, string? remarks);

    Task<List<StaffOptionDto>> GetParkStaffOptions(Guid parkId);

    Task<List<StaffOptionDto>> GetOfficeStaffOptions(Guid officeId);
}
