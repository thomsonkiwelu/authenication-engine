using conservation_backend.Config;
using conservation_backend.Features.Sections;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class SectionSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Sections.AnyAsync())
            {
                Logger.LogInformation("Seeding Section data...");

                await context.Sections.AddRangeAsync(
                    new Section
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tourism",
                        DepartmentId = Guid.Parse("90d7ced6-0c86-49a9-a36a-95e9d02a8c3a"),
                        OfficeId = Guid.Parse("72e5ebdb-3b75-44e7-b629-4a564f631de4"),
                        CreatedAt = DateTime.Now
                    },
                    new Section
                    {
                        Id = Guid.NewGuid(),
                        Name = "Less",
                        DepartmentId = Guid.Parse("90d7ced6-0c86-49a9-a36a-95e9d02a8c3a"),
                        OfficeId = Guid.Parse("72e5ebdb-3b75-44e7-b629-4a564f631de4"),
                        CreatedAt = DateTime.Now
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
