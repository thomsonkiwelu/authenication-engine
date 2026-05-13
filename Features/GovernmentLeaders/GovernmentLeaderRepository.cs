
using conservation_backend.Config;
using conservation_backend.Features.GovernmentLeaders.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.GovernmentLeaders;

public class GovernmentLeaderRepository(AppDBContext context, IUserContext userContext): IGovernmentLeaderRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<GovernmentLeader>> GetPagedData(GovernmentLeaderPaginationDto dto)
    {
        string[] searchColumns = new string[] { "FullName", "Mobile" };
        var query = _context.GovernmentLeaders
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<GovernmentLeader>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<GovernmentLeader>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.EntityId))
            query = query.Where(v => v.EntityId == Guid.Parse(dto.EntityId));
        
        if (!string.IsNullOrWhiteSpace(dto.EntityName))
            query = query.Where(v => v.EntityName == dto.EntityName);

        return await PagedList<GovernmentLeader>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<GovernmentLeader> Create(GovernmentLeader governmentLeader)
    {
        governmentLeader.CreatedBy = _userContext.GetUserId();
        _context.GovernmentLeaders.Add(governmentLeader);

        await _context.SaveChangesAsync();
        return governmentLeader;
    }

    public async Task<GovernmentLeader> GetById(Guid id)
    {
        var governmentLeader = await _context.GovernmentLeaders
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (governmentLeader is null)
            throw new KeyNotFoundException($"Government leader record not found");

        return governmentLeader;
    }

    public async Task<GovernmentLeader> Update(Guid id, GovernmentLeader governmentLeader)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""GovernmentLeaders""
            SET ""FullName"" = {governmentLeader.FullName}, ""Mobile"" = {governmentLeader.Mobile}, ""Position"" = {governmentLeader.Position}, 
            ""TelephoneNumber"" = {governmentLeader.TelephoneNumber}, ""Address"" = {governmentLeader.Address}, ""UpdatedBy"" = {_userContext.GetUserId()},
            ""EntityId"" = {governmentLeader.EntityId}, ""EntityName"" = {governmentLeader.EntityName}, ""FieldName"" = {governmentLeader.FieldName},
            ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
        ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Government leader record not found.");

        var updatedGovernmentLeader = await _context.GovernmentLeaders.FindAsync(id);
        return updatedGovernmentLeader ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var governmentLeader = await _context.GovernmentLeaders.FindAsync(id);

        if (governmentLeader is null)
            throw new KeyNotFoundException("Government leader record not found");

        governmentLeader.DeletedAt = DateTime.UtcNow;
        governmentLeader.UpdatedBy = _userContext.GetUserId();
        governmentLeader.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
}