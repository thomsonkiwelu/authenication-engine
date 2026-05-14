using System.Text.Json;
using authentication_engine.Config;
using authentication_engine.Features.Permissions;
using authentication_engine.Features.Roles.Interfaces;
using authentication_engine.Features.SystemModules;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace authentication_engine.Features.Roles
{
    public class RoleRepository(AppDBContext context, IUserContext userContext) : IRoleRepository
    {
        private readonly AppDBContext _context = context;
        private readonly IUserContext _userContext = userContext;

        public async Task<PagedList<RoleResponseDto>> GetPagedData(RolePaginationDto dto)
        {
            var connectionString = _context.Database.GetConnectionString();
            var parkIds = _userContext.GetAuthorizedParkIds(_context);
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand(
                "SELECT fn_roles(@page, @pageSize, @search, @systemApplicationId)",
                connection
            );
    
            // Add parameters with proper null handling
            command.Parameters.AddWithValue("page", dto.page);
            command.Parameters.AddWithValue("pageSize", dto.pageSize);
            command.Parameters.AddWithValue("search", NpgsqlDbType.Varchar, 
                string.IsNullOrWhiteSpace(dto.q) ? (object)DBNull.Value : dto.q);
            command.Parameters.AddWithValue("systemApplicationId", NpgsqlDbType.Varchar,
                string.IsNullOrWhiteSpace(dto.SystemApplicationId) ? (object)DBNull.Value : dto.SystemApplicationId);
            
            var jsonResult = await command.ExecuteScalarAsync() as string;
            if (string.IsNullOrWhiteSpace(jsonResult)) throw new ArgumentNullException("Failure to get roles data");
        
            var apiResponse = JsonSerializer.Deserialize<RoleSqlResponseDto>(
                jsonResult,
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                }
            );
    
            return new PagedList<RoleResponseDto>(
                apiResponse!.Data,
                dto.page,
                dto.pageSize,
                apiResponse.Meta.TotalItems
            );
        }

        public async Task<Role> Create(Role role)
        {
            role.CreatedBy = _userContext.GetUserId();
           
            _context.Roles.Add(role);

            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<(Role role, List<PermissionMinimalDto> permissions, List<SystemModuleMinimalDto> systemModules)>
            GetById(Guid id)
        {
            var role = await _context.Roles
                .Include(sa => sa.SystemApplication)
                .FirstOrDefaultAsync(s => s.Id == id);

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
            
            var systemModules = await (
                    from sm in _context.SystemModules
                    join r in _context.Roles on sm.SystemApplicationId equals r.SystemApplicationId
                    where r.Id == role.Id
                    select new SystemModuleMinimalDto
                    {
                        Id = sm.Id,
                        Name = sm.Name,
                        Slug = sm.Slug
                    }
                )
                .OrderBy(sm => sm.Name)
                .ToListAsync();

            return (role, permissions, systemModules);
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
