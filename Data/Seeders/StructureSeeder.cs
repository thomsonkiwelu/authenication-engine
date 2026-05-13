using authentication_engine.Config;
using authentication_engine.Features.Structure;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
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
