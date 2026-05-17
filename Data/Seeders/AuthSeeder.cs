using authentication_engine.Config;
using authentication_engine.Features.Departments;
using authentication_engine.Features.Roles;
using authentication_engine.Features.SystemApplications;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class AuthSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.RoleUsers.AnyAsync())
            {
                Logger.LogInformation("Seeding Authentication data ...");
                
                var users = await context.Users.ToListAsync();

                foreach (var user in users)
                {
                    var department = await context.Departments.FirstOrDefaultAsync(u => u.Code == "ICT");
                    if (department is null)
                        throw new KeyNotFoundException($"Department not found.");

                    var staff = await context.Staffs.FirstOrDefaultAsync(u => u.Email == user.Email);
                    if (staff is null)
                        throw new KeyNotFoundException($"Staff not found.");
                    
                    var role = await context.Roles.FirstOrDefaultAsync(u => u.Name == "Super Administrator");
                    if (role is null)
                        throw new KeyNotFoundException($"Role not found.");
                    
                    var systemApplication = await context.SystemApplications.FirstOrDefaultAsync(u => u.Slug == "authentication-engine");
                    if (systemApplication is null)
                        throw new KeyNotFoundException($"SystemApplication not found.");
                    
                    user.StaffId = staff.Id;
                    
                    context.DepartmentStaffs.Add(new DepartmentStaff
                    {
                        ModelType = "department",
                        DepartmentId = department.Id,
                        StaffId = staff.Id,
                        OfficeId = department.OfficeId,
                        CreatedBy = user.Id,
                        CreatedAt = DateTime.Now
                    });
                    
                    context.RoleUsers.Add(new RoleUser
                    { 
                        UserId = user.Id,
                        RoleId = role.Id,
                        CreatedBy = user.Id,
                        CreatedAt = DateTime.Now
                    });
                    
                    context.UserSystemApplications.Add(new UserSystemApplication
                    { 
                        UserId = user.Id,
                        SystemApplicationId = systemApplication.Id,
                        CreatedBy = user.Id,
                        CreatedAt = DateTime.Now
                    });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
