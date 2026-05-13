using conservation_backend.Config;
using conservation_backend.Features.Tribes.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.Tribes;

public class TribeRepository(AppDBContext context, IUserContext userContext) : ITribeRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Tribe>> GetPagedData(PaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        
        var query = _context.Tribes
            .Include(c => c.Creator)
            .Include(u => u.Updater)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Tribe>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<Tribe>.ApplySorting(query, dto.sortBy, dto.sortDesc);

        return await PagedList<Tribe>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Tribe> Create(Tribe tribe)
    {
        tribe.CreatedBy = _userContext.GetUserId();
        _context.Tribes.Add(tribe);

        await _context.SaveChangesAsync();
        return tribe;
    }

    public async Task<Tribe> GetById(Guid id)
    {
        var tribe = await _context.Tribes.FindAsync(id);

        if (tribe is null)
            throw new KeyNotFoundException($"Tribes record not found");

        return tribe;
    }

    public async Task<Tribe> Update(Guid id, Tribe tribe)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Tribes""
                SET ""Name"" = {tribe.Name}, ""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Tribe record not found.");
            
        var updated = await _context.Tribes.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var tribe = await _context.Tribes.FindAsync(id);

        if (tribe is null)
            throw new KeyNotFoundException("Tribe record not found");

        tribe.DeletedAt = DateTime.UtcNow;
        tribe.UpdatedBy = _userContext.GetUserId();
        tribe.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}