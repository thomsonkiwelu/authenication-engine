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

                var requiredModules = new List<(string Name, string Slug, string ApplicationSlugName)>
                {
                    ("Ecology", "ecology", "conservation-system"),
                    ("Community", "community", "conservation-system"),
                    ("Vet", "vet", "conservation-system"),
                    ("Setting", "setting", "conservation-system"),
                    ("Research", "research", "conservation-system"),
                    ("Law Enforcement Security", "law_enforcement_security", "conservation-system"),
                    ("User Management", "user_management", "authentication-engine"),
                };

                foreach (var module in requiredModules)
                {
                    var systemApplication = await context.SystemApplications.FirstOrDefaultAsync(u => u.Slug == module.ApplicationSlugName);
                    if (systemApplication is null)
                        throw new KeyNotFoundException($"System application not found.");
                    
                    var existingModule = await context.SystemModules
                        .FirstOrDefaultAsync(m => m.Slug == module.Slug);

                    if (existingModule == null)
                    {
                        var systemModule = new SystemModule
                        {
                            Name = module.Name,
                            Slug = module.Slug,
                            SystemApplicationId = systemApplication.Id,
                            CreatedBy = seedUserId,
                            CreatedAt = DateTime.Now,
                        };
        
                        await context.SystemModules.AddAsync(systemModule);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
