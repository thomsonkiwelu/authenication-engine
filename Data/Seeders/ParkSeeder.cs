using authentication_engine.Config;
using authentication_engine.Features.Parks;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class ParkSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Parks.AnyAsync())
            {
                Logger.LogInformation("Seeding Parks data...");

                var parks = new List<Park>
                {
                    new Park { Name = "Serengeti National Park", Code = "SENAPA", Zone = "1" },
                    new Park { Name = "Arusha National Park", Code = "ANAPA", Zone = "1" },
                    new Park { Name = "Nyerere National Park", Code = "", Zone = "2"  },
                    new Park { Name = "Kilimanjaro National Park", Code = "KINAPA", Zone = "1" },
                    new Park { Name = "Tarangire National Park", Code = "", Zone = "1"  },
                    new Park { Name = "Lake Manyara National Park", Code = "", Zone = "1" },
                    new Park { Name = "Mikumi National Park", Code = "", Zone = "1" },
                    new Park { Name = "Ruaha National Park", Code = "", Zone = "1"  },
                    new Park { Name = "Katavi National Park", Code = "",  Zone = "1" },
                    new Park { Name = "Gombe Stream National Park", Code = "", Zone = "1" },
                    new Park { Name = "Mahale Mountains National Park", Code = "", Zone = "1"  },
                    new Park { Name = "Udzungwa Mountains National Park", Code = "", Zone = "1" },
                    new Park { Name = "Kitulo National Park", Code = "", Zone = "1"  },
                    new Park { Name = "Rubondo Island National Park", Code = "", Zone = "1" },
                    new Park { Name = "Saadani National Park", Code = "", Zone = "1" },
                    new Park { Name = "Mkomazi National Park", Code = "",  Zone = "1" },
                    new Park { Name = "Burigi-Chato National Park", Code = "", Zone = "1" },
                    new Park { Name = "Ibanda-Kyerwa National Park", Code = "", Zone = "1" },
                    new Park { Name = "Rumanyika-Karagwe National Park", Code = "", Zone = "1" },
                    new Park { Name = "Ugalla River National Park", Code = "", Zone = "1" }
                };
                
                await context.Parks.AddRangeAsync(parks);
                await context.SaveChangesAsync();
            }
        }
    }
}
