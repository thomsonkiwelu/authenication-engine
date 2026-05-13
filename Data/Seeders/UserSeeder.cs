using authentication_engine.Config;
using authentication_engine.Features.Auth.Interfaces;
using authentication_engine.Features.Users;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class UserSeeder(IPasswordService passwordService) : IBaseSeeder
    {
        private readonly IPasswordService _passwordService = passwordService;

        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                Logger.LogInformation("Seeding Users data ...");
                    
                var userOne = await context.Staffs.FirstOrDefaultAsync(u => u.Email == "thomson.kiwelu@tanzaniaparks.go.tz");
                if (userOne is null)
                    throw new KeyNotFoundException($"User not found.");
                
                var userTwo = await context.Staffs.FirstOrDefaultAsync(u => u.Email == "emmanuel.birage@tanzaniaparks.go.tz");
                if (userTwo is null)
                    throw new KeyNotFoundException($"User not found.");
                
                await context.Users.AddRangeAsync(
                    new User
                    {
                        Username = "thomson.kiwelu",
                        Email = "thomson.kiwelu@tanzaniaparks.go.tz",
                        Password = _passwordService.HashPassword("Tanapa@2026"),
                        StaffId = userOne.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "emmanuel.birage",
                        Email = "emmanuel.birage@tanzaniaparks.go.tz",
                        Password = _passwordService.HashPassword("Tanapa@2026"),
                        StaffId = userTwo.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
