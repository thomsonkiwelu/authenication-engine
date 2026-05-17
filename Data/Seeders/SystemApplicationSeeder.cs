using authentication_engine.Config;
using authentication_engine.Features.SystemApplications;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class SystemApplicationSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.SystemApplications.AnyAsync())
            {
                Logger.LogInformation("Seeding System Application data ...");
    
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                
                if (user is null)
                    throw new KeyNotFoundException($"User not found.");
                
                await context.SystemApplications.AddRangeAsync(
                    new SystemApplication
                    {
                        Id = Guid.NewGuid(),
                        Name = "Authentication Engine",
                        Slug = "authentication-engine",
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    },
                    new SystemApplication
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation System",
                        Slug = "conservation-system",
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
