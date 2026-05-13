using authentication_engine.Config;
using authentication_engine.Features.Staffs;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class StaffSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Staffs.AnyAsync())
            {
                Logger.LogInformation("Seeding Staffs data ...");
                
                var rank = await context.Ranks.FirstOrDefaultAsync(u => u.Code == "CO-II");
                if (rank is null)
                    throw new KeyNotFoundException($"Rank not found.");
                
                await context.Staffs.AddRangeAsync(
                    new Staff
                    {
                        FirstName = "Emmanuel",
                        LastName = "Birage",
                        Email = "emmanuel.birage@tanzaniaparks.go.tz",
                        PhoneNumber = "0742023630",
                        Status = "1",
                        RankId =  rank.Id,
                        CreatedAt = DateTime.Now
                    },
                    new Staff
                    {
                        FirstName = "Thomson",
                        LastName = "Kiwelu",
                        Email = "thomson.kiwelu@tanzaniaparks.go.tz",
                        PhoneNumber = "0672354802",
                        Status = "1",
                        RankId =  rank.Id,
                        CreatedAt = DateTime.Now
                    }
                );
                
                await context.SaveChangesAsync();
            }
        }
    }
}
