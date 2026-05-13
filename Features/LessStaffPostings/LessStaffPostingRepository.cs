using conservation_backend.Config;
using conservation_backend.Features.LessStaffPostings.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessStaffPostings;

public class LessStaffPostingRepository(AppDBContext context, IUserContext userContext) : ILessStaffPostingRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<LessStaffPosting>> GetPagedData(LessStaffPostingPaginationDto dto)
    {
        string[] searchColumns = new[] { "Staff.FirstName", "Staff.LastName" };

        var query = _context.LessStaffPostings
            .Include(p => p.Staff)
            .ThenInclude(s => s.Rank)
            .Include(p => p.Park)
            .Include(p => p.LessOperationalZone)
            .Include(p => p.Office)
            .Include(p => p.LessRangerStation)
            .Include(p => p.LessRangerGroup)
            .Include(p => p.Creator)
            .AsNoTracking()
            .AsQueryable();

        query = query.Where(p => p.EffectiveTo == null);

        query = ApplyFilters<LessStaffPosting>.ApplySearch(query, dto.q ?? "", searchColumns);
        query = ApplyFilters<LessStaffPosting>.ApplySorting(query, dto.sortBy, dto.sortDesc);

        if (!string.IsNullOrWhiteSpace(dto.ParkId))
            query = query.Where(p => p.ParkId == Guid.Parse(dto.ParkId));

        if (!string.IsNullOrWhiteSpace(dto.OfficeId))
            query = query.Where(p => p.OfficeId == Guid.Parse(dto.OfficeId));

        if (!string.IsNullOrWhiteSpace(dto.LessOperationalZoneId))
            query = query.Where(p => p.LessOperationalZoneId == Guid.Parse(dto.LessOperationalZoneId));

        if (!string.IsNullOrWhiteSpace(dto.LessRangerStationId))
            query = query.Where(p => p.LessRangerStationId == Guid.Parse(dto.LessRangerStationId));

        if (!string.IsNullOrWhiteSpace(dto.LessRangerGroupId))
            query = query.Where(p => p.LessRangerGroupId == Guid.Parse(dto.LessRangerGroupId));

        if (!string.IsNullOrWhiteSpace(dto.StaffId))
            query = query.Where(p => p.StaffId == Guid.Parse(dto.StaffId));

        return await PagedList<LessStaffPosting>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<LessStaffPosting?> GetActivePostingByStaffId(Guid staffId)
    {
        return await _context.LessStaffPostings
            .Where(p => p.StaffId == staffId && p.EffectiveTo == null)
            .OrderByDescending(p => p.EffectiveFrom)
            .FirstOrDefaultAsync();
    }

    public async Task CloseActivePostingByStaffId(Guid staffId, string? remarks)
    {
        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        await _context.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE "LessStaffPostings"
            SET "EffectiveTo" = {now}, "Remarks" = COALESCE({remarks}, "Remarks"),
                "UpdatedBy" = {userId}, "UpdatedAt" = {now}
            WHERE "StaffId" = {staffId} AND "EffectiveTo" IS NULL AND "DeletedAt" IS NULL;
        """);
    }

    public async Task<LessStaffPosting> Create(LessStaffPosting posting)
    {
        posting.CreatedBy = _userContext.GetUserId();
        _context.LessStaffPostings.Add(posting);
        await _context.SaveChangesAsync();
        return posting;
    }

    public async Task<bool> ClosePosting(Guid id, string? remarks)
    {
        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE "LessStaffPostings"
            SET "EffectiveTo" = {now}, "Remarks" = COALESCE({remarks}, "Remarks"),
                "UpdatedBy" = {userId}, "UpdatedAt" = {now}
            WHERE "Id" = {id} AND "DeletedAt" IS NULL;
        """);

        return rows > 0;
    }

    public async Task<List<StaffOptionDto>> GetParkStaffOptions(Guid parkId)
    {
        var lessSectionIdsQuery = _context.Sections
            .AsNoTracking()
            .Where(s => s.DeletedAt == null)
            .Where(s => s.Office.ParkId == parkId)
            .Where(s => EF.Functions.ILike(s.Code, "LESS") || EF.Functions.ILike(s.Name, "LESS"))
            .Select(s => s.Id);

        var lessSectionExists = await lessSectionIdsQuery.AnyAsync();
        if (!lessSectionExists)
            return new List<StaffOptionDto>();

        var staffIdsQuery = _context.DepartmentStaffs
            .AsNoTracking()
            .Where(ds => ds.DeletedAt == null)
            .Where(ds => ds.Office.ParkId == parkId)
            .Where(ds => ds.ModelType == "section")
            .Where(ds => lessSectionIdsQuery.Contains(ds.DepartmentId))
            .Select(ds => ds.StaffId)
            .Distinct();

        var activeStaffIds = _context.LessStaffPostings
            .AsNoTracking()
            .Where(p => p.EffectiveTo == null)
            .Select(p => p.StaffId)
            .Distinct();

        var data = await _context.Staffs
            .AsNoTracking()
            .Where(s => staffIdsQuery.Contains(s.Id))
            .Where(s => !activeStaffIds.Contains(s.Id))
            .Select(s => new StaffOptionDto
            {
                Id = s.Id,
                Name = s.FirstName + " " + s.LastName,
                RankId = s.RankId,
                RankName = s.Rank.Name,
            })
            .OrderBy(x => x.Name)
            .ToListAsync();

        return data;
    }

    public async Task<List<StaffOptionDto>> GetOfficeStaffOptions(Guid officeId)
    {
        var lessSectionIdsQuery = _context.Sections
            .AsNoTracking()
            .Where(s => s.DeletedAt == null)
            .Where(s => s.OfficeId == officeId)
            .Where(s => EF.Functions.ILike(s.Code, "LESS") || EF.Functions.ILike(s.Name, "LESS"))
            .Select(s => s.Id);

        var lessSectionExists = await lessSectionIdsQuery.AnyAsync();
        if (!lessSectionExists)
            return new List<StaffOptionDto>();

        var staffIdsQuery = _context.DepartmentStaffs
            .AsNoTracking()
            .Where(ds => ds.DeletedAt == null)
            .Where(ds => ds.OfficeId == officeId)
            .Where(ds => ds.ModelType == "section")
            .Where(ds => lessSectionIdsQuery.Contains(ds.DepartmentId))
            .Select(ds => ds.StaffId)
            .Distinct();

        var activeStaffIds = _context.LessStaffPostings
            .AsNoTracking()
            .Where(p => p.EffectiveTo == null)
            .Select(p => p.StaffId)
            .Distinct();

        var data = await _context.Staffs
            .AsNoTracking()
            .Where(s => staffIdsQuery.Contains(s.Id))
            .Where(s => !activeStaffIds.Contains(s.Id))
            .Select(s => new StaffOptionDto
            {
                Id = s.Id,
                Name = s.FirstName + " " + s.LastName,
                RankId = s.RankId,
                RankName = s.Rank.Name,
            })
            .OrderBy(x => x.Name)
            .ToListAsync();

        return data;
    }
}
