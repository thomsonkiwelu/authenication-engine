using conservation_backend.Config;
using conservation_backend.Features.Sections;
using conservation_backend.Features.Units;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class UnitSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Units.AnyAsync())
            {
                Logger.LogInformation("Seeding Unit data...");

                await context.Units.AddRangeAsync(
                    new Unit
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tourism Unit",
                        DepartmentId = Guid.Parse("dc78a0ad-3caf-4d5c-8360-72d69ae177d3"),
                        OfficeId = Guid.Parse("72e5ebdb-3b75-44e7-b629-4a564f631de4"),
                        CreatedAt = DateTime.Now
                    },
                    new Unit
                    {
                        Id = Guid.NewGuid(),
                        Name = "Less Unit",
                        SectionId = Guid.Parse("8f0dc56a-5e10-4b77-8dab-b14800a7b798"),
                        OfficeId = Guid.Parse("72e5ebdb-3b75-44e7-b629-4a564f631de4"),
                        CreatedAt = DateTime.Now
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
