using authentication_engine.Config;
using authentication_engine.Features.Roles;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class RoleSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            Logger.LogInformation("Seeding Roles data ...");

            var seedUserId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                             ?? await context.Users.FirstOrDefaultAsync())
                ?.Id;

            var role = await context.Roles.FirstOrDefaultAsync(u => u.Name == "Super Admin");

            if (role is null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Super Admin",
                    Description = "Manage all users in application",
                    CreatedBy = seedUserId,
                    CreatedAt = DateTime.Now
                };

                context.Roles.Add(role);
                await context.SaveChangesAsync();
            }

            var permissionsWithModuleSlug = await context.Permissions
                .Join(
                    context.SystemModules,
                    p => p.SystemModuleId,
                    sm => sm.Id,
                    (p, sm) => new { PermissionId = p.Id, ModuleSlug = sm.Slug }
                )
                .ToListAsync();

            var existingPermissionIds = await context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            var missingRolePermissions = permissionsWithModuleSlug
                .Where(p => !existingPermissionIds.Contains(p.PermissionId))
                .Select(p => new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = p.PermissionId,
                    ModuleName = p.ModuleSlug,
                    CreatedBy = seedUserId,
                    CreatedAt = DateTime.Now
                })
                .ToList();

            if (missingRolePermissions.Count > 0)
            {
                await context.RolePermissions.AddRangeAsync(missingRolePermissions);
                await context.SaveChangesAsync();
            }
        }
    }
}
