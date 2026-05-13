using conservation_backend.Config;
using conservation_backend.Features.LessActivities.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessActivities;

public class LessActivityRepository(AppDBContext context, IUserContext userContext) : ILessActivityRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<LessActivity>> GetPagedData(LessActivityPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Key" };
        var query = _context.LessActivities
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<LessActivity>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<LessActivity>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.Category))
            query = query.Where(v => v.Category == dto.Category);

        return await PagedList<LessActivity>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<LessActivity> Create(LessActivity lessActivity)
    {
        lessActivity.CreatedBy = _userContext.GetUserId();
        _context.LessActivities.Add(lessActivity);

        await _context.SaveChangesAsync();
        return lessActivity;
    }

    public async Task<LessActivity> GetById(Guid id)
    {
        var lessActivity = await _context.LessActivities.FindAsync(id);

        if (lessActivity is null)
            throw new KeyNotFoundException($"Less activity record not found");

        return lessActivity;
    }

    public async Task<LessActivity> Update(Guid id, LessActivity lessActivity)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""LessActivities""
            SET ""Label"" = {lessActivity.Label}, ""Key"" = {lessActivity.Key},""UpdatedBy"" = {_userContext.GetUserId()},
            ""Category"" = {lessActivity.Category}, ""IsActive"" = {lessActivity.IsActive},""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Less activity record not found.");
            
        var updated = await _context.LessActivities.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var lessActivity = await _context.LessActivities.FindAsync(id);

        if (lessActivity is null)
            throw new KeyNotFoundException("Less activity record not found");

        lessActivity.DeletedAt = DateTime.UtcNow;
        lessActivity.UpdatedBy = _userContext.GetUserId();
        lessActivity.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}