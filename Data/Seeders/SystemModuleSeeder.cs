using conservation_backend.Config;
using conservation_backend.Features.SystemModules;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class SystemModuleSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
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
                ("Law Enforcement & Security", "less"),
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

            if (missingModules.Count > 0)
            {
                await context.SystemModules.AddRangeAsync(missingModules);
                await context.SaveChangesAsync();
            }
        }
    }
}
