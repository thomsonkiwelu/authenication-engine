using conservation_backend.Config;
using conservation_backend.Features.SystemModules.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.SystemModules;

public class SystemModuleRepository(AppDBContext context): ISystemModuleRepository
{
    private readonly AppDBContext _context = context;

    public async Task<PagedList<SystemModule>> GetPagedData(PaginationDto dto)
    {
        string[] searchColumns = new string[] { "ModelType" };
        var query = _context.SystemModules.AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<SystemModule>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<SystemModule>.ApplySorting(query, "name", false);

        return await PagedList<SystemModule>.CreateAsync(query, dto.page, dto.pageSize);
    }
    
    public async Task<SystemModule> GetById(Guid id)
    {
        var systemModule = await _context.SystemModules.FindAsync(id);

        if (systemModule is null)
            throw new KeyNotFoundException($"System module record not found");

        return systemModule;
    }
}