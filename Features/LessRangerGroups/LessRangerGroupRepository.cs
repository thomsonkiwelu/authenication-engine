using authentication_engine.Config;
using authentication_engine.Features.LessRangerGroups.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.LessRangerGroups;

public class LessRangerGroupRepository(AppDBContext context, IUserContext userContext) : ILessRangerGroupRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<LessRangerGroup>> GetPagedData(LessRangerGroupPaginationDto dto)
    {
        string[] searchColumns = new[] { "Name" };

        var query = _context.LessRangerGroups
            .Include(g => g.LessRangerStation)
            .Include(g => g.Creator)
            .AsNoTracking()
            .AsQueryable();

        query = ApplyFilters<LessRangerGroup>.ApplySearch(query, dto.q ?? "", searchColumns);
        query = ApplyFilters<LessRangerGroup>.ApplySorting(query, dto.sortBy, dto.sortDesc);

        if (!string.IsNullOrWhiteSpace(dto.LessRangerStationId))
            query = query.Where(g => g.LessRangerStationId == Guid.Parse(dto.LessRangerStationId));

        return await PagedList<LessRangerGroup>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<LessRangerGroup> Create(LessRangerGroup group)
    {
        group.CreatedBy = _userContext.GetUserId();
        _context.LessRangerGroups.Add(group);

        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<LessRangerGroup> GetById(Guid id)
    {
        var result = await _context.LessRangerGroups
            .Include(g => g.LessRangerStation)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (result is null)
            throw new KeyNotFoundException("Group record not found");

        return result;
    }

    public async Task<LessRangerGroup> Update(Guid id, LessRangerGroup group)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""LessRangerGroups""
                SET ""Name"" = {group.Name}, ""Code"" = {group.Code}, ""LessRangerStationId"" = {group.LessRangerStationId},
                    ""UpdatedBy"" = {_userContext.GetUserId()}, ""UpdatedAt"" = {DateTime.UtcNow}
                WHERE ""Id"" = {id};
            ");

        if (rows == 0)
            throw new KeyNotFoundException("Group record not found.");

        var updated = await _context.LessRangerGroups
            .Include(g => g.LessRangerStation)
            .FirstOrDefaultAsync(g => g.Id == id);

        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var group = await _context.LessRangerGroups.FindAsync(id);

        if (group is null)
            throw new KeyNotFoundException("Group record not found");

        group.DeletedAt = DateTime.UtcNow;
        group.UpdatedBy = _userContext.GetUserId();
        group.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
