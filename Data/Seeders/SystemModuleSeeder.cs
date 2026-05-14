using authentication_engine.Config;
using authentication_engine.Features.SystemModules;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class SystemModuleSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.SystemModules.AnyAsync())
            {
                Logger.LogInformation("Seeding System Modules data ...");

                var seedUserId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                                 ?? await context.Users.FirstOrDefaultAsync())
                    ?.Id;

                var requiredModules = new List<(string Name, string Slug)>
                {
                    ("Ecology", "ecology"),
                    ("Community", "community"),
                    ("Vet", "vet"),
                    ("Setting", "setting"),
                    ("Research", "research"),
                    ("Law Enforcement Security", "law_enforcement_security"),
                    ("User Management", "user_management"),
                };

                var existingSlugs = await context.SystemModules
                    .Select(m => m.Slug)
                    .ToListAsync();

                var missingModules = requiredModules
                    .Where(m => !existingSlugs.Contains(m.Slug))
                    .Select(m => new SystemModule
                    {
                        Name = m.Name,
                        Slug = m.Slug,
                        CreatedBy = seedUserId,
                        CreatedAt = DateTime.Now,
                    })
                    .ToList();
                
                    await context.SystemModules.AddRangeAsync(missingModules);
                    await context.SaveChangesAsync();
            }
        }
    }
}
