using authentication_engine.Config;
using authentication_engine.Features.Departments;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class DepartmentSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Departments.AnyAsync())
            {
                Logger.LogInformation("Seeding Department data...");
                
                var office = await context.Offices.FirstOrDefaultAsync(u => u.Code == "HQ");
                if (office is null)
                    throw new KeyNotFoundException($"Office name is not found.");
                
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                if (user is null)
                    throw new KeyNotFoundException($"User not found.");

                await context.Departments.AddRangeAsync(
                    new Department
                    {
                        Id = Guid.NewGuid(),
                        Name = "Information Communication Technology",
                        Code = "ICT",
                        OfficeId = office.Id,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
