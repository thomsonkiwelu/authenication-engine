using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.Wards;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class WardSeeder : IBaseSeeder
    {
        private record WardSeederDto(
            string Region,
            string District,
            string Ward
        );
        
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Wards.AnyAsync())
            {
                Logger.LogInformation("Seeding Wards data ...");
                
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                if (user is null)
                    throw new KeyNotFoundException($"User not found.");
                
                // Get the path to wards.json
                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData", "wards.json");
                if (!File.Exists(jsonPath))
                    throw new FileNotFoundException($"wards.json not found at: {jsonPath}");
                
                var jsonContent = await File.ReadAllTextAsync(jsonPath);
                var wards = JsonSerializer.Deserialize<List<WardSeederDto>>(jsonContent);
               
                foreach (var ward in wards)
                {
                    var district = await context.Districts.FirstOrDefaultAsync(u => u.Name == ward.District);
                    if (district is null)
                        throw new KeyNotFoundException($"District not found: {ward.District} - skipping...");
                    
                    await context.Wards.AddAsync(new Ward
                    {
                        Name = ward.Ward,
                        RegionId = district.RegionId,
                        DistrictId = district.Id,
                        CreatedBy = user.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}
