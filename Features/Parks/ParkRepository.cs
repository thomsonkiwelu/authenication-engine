using authentication_engine.Config;
using authentication_engine.Features.Parks.Interfaces;
using authentication_engine.Features.Users;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Parks;

public class ParkRepository(AppDBContext context, IUserContext userContext): IParkRepository
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    
    public async Task<PagedList<Park>> GetPagedData(ParkPaginationDto dto)
    {
        string[] searchColumns = new string[] { "Name" };
        var query = _context.Parks.AsNoTracking().AsQueryable();

        //Apply search filter
        query = ApplyFilters<Park>.ApplySearch(query, dto.q ?? "", searchColumns);
        
        //Apply sorting filter
        query = ApplyFilters<Park>.ApplySorting(query, dto.sortBy, dto.sortDesc);
        
        //Apply Park filter
        var parkIds = _userContext.GetAuthorizedParkIds(_context);
        if (parkIds.Any())
            query = query.Where(v => parkIds.Contains(v.Id));
            
        if (!string.IsNullOrWhiteSpace(dto.Zone))
            query = query.Where(v => v.Zone == dto.Zone);

        return await PagedList<Park>.CreateAsync(query, dto.page, dto.pageSize);
    }

    public async Task<Park> Create(Park park)
    {
        park.CreatedBy = _userContext.GetUserId();
        _context.Parks.Add(park);

        await _context.SaveChangesAsync();
        return park;
    }

    public async Task<Park> GetById(Guid id)
    {
        var park = await _context.Parks.FindAsync(id);
        if (park is null)
            throw new KeyNotFoundException($"Park record not found");

        return park;
    }

    public async Task<Park> Update(Guid id, Park park)
    {
        var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Parks""
                SET ""Name"" = {park.Name}, ""Code"" = {park.Code}, ""Zone"" = {park.Zone},""UpdatedBy"" = {_userContext.GetUserId()},
                ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
        if (rows == 0)
            throw new KeyNotFoundException("Park record not found.");

        var updatedPark = await _context.Parks.FindAsync(id);
        return updatedPark ?? throw new KeyNotFoundException("Failed to retrieve updated record");
    }

    public async Task<bool> Delete(Guid id)
    {
        var park = await _context.Parks.FindAsync(id);

        if (park is null)
            throw new KeyNotFoundException("Park record not found");

        park.DeletedAt = DateTime.UtcNow;
        park.UpdatedBy = _userContext.GetUserId();
        park.UpdatedAt = DateTime.UtcNow;
         
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AssignParkToUser(UserPark userPark)
    {
        userPark.CreatedBy = _userContext.GetUserId();
        _context.UserParks.Add(userPark);
            
        await _context.SaveChangesAsync();
        return true;
    }
        
    public async Task<bool> UnassignParkToUser(UserPark userPark)
    {
        var userParkExist = _context.UserParks
            .Where(rp => rp.ParkId == userPark.ParkId)
            .Where(rp => rp.UserId == userPark.UserId);

        _context.UserParks.RemoveRange(userParkExist);
        await _context.SaveChangesAsync();
            
        return true;
    }
}