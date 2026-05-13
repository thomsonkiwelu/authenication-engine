using conservation_backend.Config;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class MethodologySeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            //if (!await context.Methodologies.AnyAsync())
            //{
            //    Logger.LogInformation("Seeding Methodology data ...");


            //    await context.SaveChangesAsync();
            //}   
        }
    }
}
