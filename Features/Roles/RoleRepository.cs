using authentication_engine.Config;
using authentication_engine.Features.Permissions;
using authentication_engine.Features.Roles.Interfaces;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Features.Roles
{
    public class RoleRepository(AppDBContext context, IUserContext userContext) : IRoleRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<Role>> GetPagedData(RolePaginationDto dto)
        {
            string[] searchColumns = new string[] { "Name" };
            var query = _context.Roles
                .Include(s => s.SystemApplication)
                .Include(c => c.Creator)
                .Include(u => u.Updater)
                .AsNoTracking().AsQueryable();

            //Apply search filter
            query = ApplyFilters<Role>.ApplySearch(query, dto.q ?? "", searchColumns);

            //Apply sorting filter
            query = ApplyFilters<Role>.ApplySorting(query, dto.sortBy, dto.sortDesc);
            
            if (!string.IsNullOrWhiteSpace(dto.SystemApplicationId))
                query = query.Where(v => v.SystemApplicationId == Guid.Parse(dto.SystemApplicationId));

            return await PagedList<Role>.CreateAsync(query, dto.page, dto.pageSize);
        }

        public async Task<Role> Create(Role role)
        {
            role.CreatedBy = _userContext.GetUserId();
           
            _context.Roles.Add(role);

            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<(Role role, List<PermissionMinimalDto> permissions)> 
            GetById(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role is null)
                throw new KeyNotFoundException($"Role record not found");

            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Include(rp => rp.Permission)
                .Select(rp => new PermissionMinimalDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Action = rp.Permission.Action
                })
           .ToListAsync();

            return (role, permissions);
        }

        public async Task<Role> Update(Guid id, Role role)
        {
            var existingRole = await _context.Roles.FindAsync(id);

            if (existingRole is null)
                throw new KeyNotFoundException($"Role record not found");

            role.Id = existingRole.Id;
            role.CreatedAt = existingRole.CreatedAt;
            role.CreatedBy = existingRole.CreatedBy;
            role.UpdatedBy = _userContext.GetUserId();
            role.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(existingRole).CurrentValues.SetValues(role);
            await _context.SaveChangesAsync();

            return existingRole;
        }

        public async Task<bool> Delete(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role is null)
                throw new KeyNotFoundException("Role record not found");

            role.DeletedAt = DateTime.UtcNow;
            role.UpdatedBy = _userContext.GetUserId();
            role.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRolePermissions(AssignRolePermissionRequest dto)
        {
            var existingPermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == dto.RoleId)
                .Where(rp => rp.ModuleName == dto.ModuleName);
            _context.RolePermissions.RemoveRange(existingPermissions);

            var rolePermissions = dto.PermissionIds.Select(permissionId => new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = Guid.Parse(permissionId),
                ModuleName = dto.ModuleName,
                CreatedBy = _userContext.GetUserId()
            }).ToList();

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<bool> AssignRoleToUser(RoleUser roleUser)
        {
            roleUser.CreatedBy = _userContext.GetUserId();
            _context.RoleUsers.Add(roleUser);
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UnassignRoleToUser(RoleUser roleUser)
        {
            var roleUserExist = _context.RoleUsers
                .Where(rp => rp.RoleId == roleUser.RoleId)
                .Where(rp => rp.UserId == roleUser.UserId);

            _context.RoleUsers.RemoveRange(roleUserExist);
            await _context.SaveChangesAsync();
            
            return true;
        }

    }
}
