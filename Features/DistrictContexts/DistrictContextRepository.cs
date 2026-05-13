using conservation_backend.Config;
using conservation_backend.Features.DistrictContexts.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.DistrictContexts;

public class DistrictContextRepository(AppDBContext context, IUserContext userContext): IDistrictContextRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<DistrictContext>> GetPagedData(DistrictContextPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.DistrictContexts
            .Include(u => u.Creator)
            .AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<DistrictContext>.ApplySearch(query, dto.q ?? "", searchColumns);

        //Apply sorting filter
        query = ApplyFilters<DistrictContext>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        if (!string.IsNullOrWhiteSpace(dto.DistrictProfileId))
            query = query.Where(v => v.DistrictProfileId == Guid.Parse(dto.DistrictProfileId));

        return await PagedList<DistrictContext>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<DistrictContext> Create(DistrictContext districtContext)
    {
        districtContext.CreatedBy = _userContext.GetUserId();
        _context.DistrictContexts.Add(districtContext);

        await _context.SaveChangesAsync();
        return districtContext;
    }

    public async Task<DistrictContext> GetById(Guid id)
    {
        var districtContext = await _context.DistrictContexts
            .Include(u => u.Creator)
            .Include(u => u.Updater)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (districtContext is null)
            throw new KeyNotFoundException($"District context record not found");

        return districtContext;
    }

    public async Task<DistrictContext> Update(Guid id, DistrictContext districtContext)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""DistrictContexts""
                SET ""DistrictProfileId"" = {districtContext.DistrictProfileId}, ""Name"" = {districtContext.Name}, ""Description"" = {districtContext.Description}, 
                ""UpdatedBy"" = {_userContext.GetUserId()},""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("District context record not found.");

        var updatedDistrictContext = await _context.DistrictContexts.FindAsync(id);
        return updatedDistrictContext ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var districtContext = await _context.DistrictContexts.FindAsync(id);

        if (districtContext is null)
            throw new KeyNotFoundException("District context record not found");

        districtContext.DeletedAt = DateTime.UtcNow;
        districtContext.UpdatedBy = _userContext.GetUserId();
        districtContext.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<DevelopmentOrganization> CreateOrganization(DevelopmentOrganization developmentOrganization)
    {
        developmentOrganization.CreatedBy = _userContext.GetUserId();
        _context.DevelopmentOrganizations.Add(developmentOrganization);

        await _context.SaveChangesAsync();
        return developmentOrganization;
    }
    
    public async Task<DevelopmentOrganization> UpdateOrganization(Guid id, DevelopmentOrganization developmentOrganization)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""DevelopmentOrganizations""
                SET ""DistrictProfileId"" = {developmentOrganization.DistrictProfileId}, ""Name"" = {developmentOrganization.Name}, ""TimeOfOperation"" = {developmentOrganization.TimeOfOperation}, 
                ""AreaOfOperation"" = {developmentOrganization.AreaOfOperation}, ""TelephoneNumber"" = {developmentOrganization.TelephoneNumber}, ""TypeOfOperation"" = {developmentOrganization.TypeOfOperation},
               ""ContactPersonName"" = {developmentOrganization.ContactPersonName}, ""ContactPersonMobile"" = {developmentOrganization.ContactPersonMobile}, ""Comment"" = {developmentOrganization.Comment},
               ""Address"" = {developmentOrganization.Address}, ""UpdatedBy"" = {_userContext.GetUserId()},""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Development organization record not found.");

        var updatedDevelopmentOrganization = await _context.DevelopmentOrganizations.FindAsync(id);
        return updatedDevelopmentOrganization ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }
    
    public async Task<bool> DeleteOrganization(Guid id)
    {
        var developmentOrganization = await _context.DevelopmentOrganizations.FindAsync(id);

        if (developmentOrganization is null)
            throw new KeyNotFoundException("Development organization record not found");

        developmentOrganization.DeletedAt = DateTime.UtcNow;
        developmentOrganization.UpdatedBy = _userContext.GetUserId();
        developmentOrganization.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
}