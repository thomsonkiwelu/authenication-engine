using conservation_backend.Config;
using conservation_backend.Features.Structure;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class StructureSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Structures.AnyAsync())
            {
                Logger.LogInformation("Seeding Structure data ...");
    
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                
                if (user is null)
                    throw new KeyNotFoundException($"User not found for name.");
                
                await context.Structures.AddRangeAsync(
                    new StructureEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tanapa HQ",
                        Level = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    },
                    new StructureEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Zone",
                        Level = 2,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    },
                    new StructureEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Parks",
                        Level = 3,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
