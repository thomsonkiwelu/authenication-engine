using conservation_backend.Config;
using conservation_backend.Features.Ranks;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class RankSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Ranks.AnyAsync())
            {
                Logger.LogInformation("Seeding Ranks data ...");
                
                await context.Ranks.AddRangeAsync(
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation Commissioner",
                        Code = "CC",
                        Category = "officer",
                        Level = 1,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Deputy Conservation Commissioner",
                        Code = "DCC",
                        Category = "officer",
                        Level = 2,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Senior Assistant Conservation Commissioner",
                        Code = "SACC",
                        Category = "officer",
                        Level = 3,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Assistant Conservation Commissioner",
                        Code = "ACC",
                        Category = "officer",
                        Level = 4,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Principal Conservator",
                        Code = "PC",
                        Category = "officer",
                        Level = 5,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Senior Conservation",
                        Code = "SC",
                        Category = "officer",
                        Level = 6,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation officer I",
                        Code = "CO-I",
                        Category = "officer",
                        Level = 7,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation officer II",
                        Code = "CO-II",
                        Category = "officer",
                        Level = 8,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Principal Conservation Ranger I",
                        Code = "PCR-I",
                        Category = "ranger",
                        Level = 9,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Principal Conservation Ranger II",
                        Code = "PCR-II",
                        Category = "ranger",
                        Level = 10,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Senior Conservation Ranger",
                        Code = "SCR",
                        Category = "ranger",
                        Level = 11,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation Ranger I",
                        Code = "CR-I",
                        Category = "ranger",
                        Level = 12,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation Ranger II",
                        Code = "CR-II",
                        Category = "ranger",
                        Level = 13,
                        CreatedAt = DateTime.Now
                    },
                    new Rank
                    {
                        Id = Guid.NewGuid(),
                        Name = "Conservation Ranger III",
                        Code = "CR-III",
                        Category = "ranger",
                        Level = 14,
                        CreatedAt = DateTime.Now
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
