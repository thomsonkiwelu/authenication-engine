using conservation_backend.Config;
using conservation_backend.Features.Regions;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class RegionSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Regions.AnyAsync())
            {
                Logger.LogInformation("Seeding Regions data ...");
                
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                if (user is null)
                    throw new KeyNotFoundException($"User not found.");
                
                await context.Regions.AddRangeAsync(
                    new Region { Id = Guid.NewGuid(), Name = "Arusha", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Kilimanjaro", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Manyara", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Tanga", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Mara", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Mwanza", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Shinyanga", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Simiyu", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Kagera", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Geita", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Dodoma", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Singida", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Tabora", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Dar es Salaam", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Pwani", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Morogoro", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Lindi", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Mtwara", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Iringa", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Mbeya", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Njombe", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Rukwa", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Katavi", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Songwe", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Kigoma", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Ruvuma", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Zanzibar Urban/West", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Zanzibar North", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Zanzibar South", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Pemba North", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow },
                    new Region { Id = Guid.NewGuid(), Name = "Pemba South", CreatedBy = user.Id, CreatedAt = DateTime.UtcNow }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
