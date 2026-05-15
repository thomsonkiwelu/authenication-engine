using authentication_engine.Config;
using authentication_engine.Features.SystemApplications.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.SystemApplications
{
    public class SystemApplicationRepository(AppDBContext context, IUserContext userContext) : ISystemApplicationRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;
      
        public async Task<PagedList<SystemApplication>> GetPagedData(PaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.SystemApplications
                .Include(c => c.Creator)
                .Include(u => u.Updater)
                .AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<SystemApplication>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<SystemApplication>.ApplySorting(query, dto.sortBy, dto.sortDesc);

            return await PagedList<SystemApplication>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<SystemApplication> Create(SystemApplication systemApplication)
        {
            systemApplication.CreatedBy = _userContext.GetUserId();
            _context.SystemApplications.Add(systemApplication);

            await _context.SaveChangesAsync();
            return systemApplication;
        }

        public async Task<SystemApplication> GetById(Guid id)
        {
            var systemApplication = await _context.SystemApplications.FindAsync(id);

            if (systemApplication is null)
                throw new KeyNotFoundException($"System application record not found");

            return systemApplication;
        }

        public async Task<SystemApplication> Update(Guid id, SystemApplication systemApplication)
        {
            var rows = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""SystemApplications""
                SET ""Name"" = {systemApplication.Name}, ""Url"" = {systemApplication.Url},""UpdatedBy"" = {_userContext.GetUserId()},
                ""ApiKey"" = {systemApplication.ApiKey}, ""UpdatedAt"" = {DateTime.UtcNow} WHERE ""Id"" = {id};
            ");
            
            if (rows == 0)
                throw new KeyNotFoundException("System application record not found.");
            
            var updated = await _context.SystemApplications.FindAsync(id);
            return updated ?? throw new KeyNotFoundException("Failed to retrieve updated record");
        }

        public async Task<bool> Delete(Guid id)
        {
            var rank = await _context.SystemApplications.FindAsync(id);

            if (rank is null)
                throw new KeyNotFoundException("System application record not found");

            rank.DeletedAt = DateTime.UtcNow;
            rank.UpdatedBy = _userContext.GetUserId();
            rank.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> AssignSystemApplicationToUser(UserSystemApplication userSystemApplication)
        {
            userSystemApplication.CreatedBy = _userContext.GetUserId();
            _context.UserSystemApplications.Add(userSystemApplication);
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UnassignSystemApplicationToUser(UserSystemApplication userSystemApplication)
        {
            var userSystemApplicationExist = _context.UserSystemApplications
                .Where(rp => rp.SystemApplicationId == userSystemApplication.SystemApplicationId)
                .Where(rp => rp.UserId == userSystemApplication.UserId);

            _context.UserSystemApplications.RemoveRange(userSystemApplicationExist);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
