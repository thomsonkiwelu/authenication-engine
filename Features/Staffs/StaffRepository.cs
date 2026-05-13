using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.Staffs.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Staffs;

public class StaffRepository(AppDBContext context, IUserContext userContext) : IStaffRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Staff>> GetPagedData(StaffPaginationDto dto)
    {
        string[] searchColumns = new string[] { "FirstName", "LastName" };
        var query = _context.Staffs.Include(r => r.Rank).AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Staff>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Staff>.ApplySorting(query, dto.sortBy, dto.sortDesc);

        if (!string.IsNullOrWhiteSpace(dto.RankId))
            query = query.Where(v => v.RankId == Guid.Parse(dto.RankId));

        return await PagedList<Staff>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Staff> Create(Staff staff)
    {
        staff.CreatedBy = _userContext.GetUserId();
        _context.Staffs.Add(staff);

        await _context.SaveChangesAsync();
        return staff;
    }

    public async Task<Staff> GetById(Guid id)
    {
        var staff = await _context.Staffs
            .Include(s => s.Rank)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (staff is null)
            throw new KeyNotFoundException($"Staff record not found");

        return staff;
    }

    public async Task<Staff> Update(Guid id, Staff staff)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Staffs""
                SET ""FirstName"" = {staff.FirstName},""LastName"" = {staff.LastName},""Email"" = {staff.Email},""PhoneNumber"" = {staff.PhoneNumber},
                    ""Status"" = {staff.Status},""UpdatedBy"" = {_userContext.GetUserId()},""RankId"" = {staff.RankId},""UpdatedAt"" = {DateTime.UtcNow}
                WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Staff record not found.");
            
        var updatedStaff = await _context.Staffs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            
        return updatedStaff ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var staff = await _context.Staffs.FindAsync(id);
            
        if (staff is null)
            throw new KeyNotFoundException("Staff record not found");

        staff.DeletedAt = DateTime.UtcNow;
        staff.UpdatedBy = _userContext.GetUserId();
        staff.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<OrganizationContextDto> GetStaffsWithOrganizationContext(Guid officeId)
    {
        var dataSql = "SELECT * FROM fun_get_staffs_organization_context({0})";

        var results = await _context.Database.SqlQueryRaw<string>(dataSql, officeId)
            .ToListAsync();

        var jsonString = results.FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(jsonString))
            return new OrganizationContextDto();

        var result = JsonSerializer.Deserialize<OrganizationContextDto>(
            jsonString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        return result ?? new OrganizationContextDto();
    }
}