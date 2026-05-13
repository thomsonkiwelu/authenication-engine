using authentication_engine.Config;
using authentication_engine.Features.SystemModules.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.SystemModules;

public class SystemModuleRepository(AppDBContext context, IUserContext userContext): ISystemModuleRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<PagedList<SystemModule>> GetPagedData(SystemModulePaginationDto dto)
    {
        string[] searchColumns = new string[] { "ModelType" };
        var query = _context.SystemModules
            .Include(s => s.SystemApplication)
            .Include(c => c.Creator)
            .Include(u => u.Updater)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<SystemModule>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<SystemModule>.ApplySorting(query, "name", false);
        
        if (!string.IsNullOrWhiteSpace(dto.SystemApplicationId))
            query = query.Where(v => v.SystemApplicationId == Guid.Parse(dto.SystemApplicationId));

        return await PagedList<SystemModule>.CreateAsync(query, dto.page, dto.pageSize);
    }
    
    public async Task<SystemModule> Create(SystemModule systemModule)
    {
        systemModule.CreatedBy = _userContext.GetUserId();
        _context.SystemModules.Add(systemModule);

        await _context.SaveChangesAsync();
        return systemModule;
    }
    
    public async Task<SystemModule> GetById(Guid id)
    {
        var systemModule = await _context.SystemModules.FindAsync(id);

        if (systemModule is null)
            throw new KeyNotFoundException($"System module record not found");

        return systemModule;
    }
    
    public async Task<SystemModule> Update(Guid id, SystemModule systemModule)
    {
        Console.WriteLine(systemModule.Slug);
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""SystemModules""
                SET ""Name"" = {systemModule.Name}, ""Slug"" = {systemModule.Slug},""UpdatedBy"" = {_userContext.GetUserId()},
                ""SystemApplicationId"" = {systemModule.SystemApplicationId}, ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("System module record not found.");
            
        var updated = await _context.SystemModules.FindAsync(id);
        return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var systemModule = await _context.SystemModules.FindAsync(id);

        if (systemModule is null)
            throw new KeyNotFoundException("System module record not found");

        systemModule.DeletedAt = DateTime.UtcNow;
        systemModule.UpdatedBy = _userContext.GetUserId();
        systemModule.UpdatedAt = DateTime.UtcNow;
            
        await _context.SaveChangesAsync();
        return true;
    }
}