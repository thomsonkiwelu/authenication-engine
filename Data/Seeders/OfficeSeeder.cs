using authentication_engine.Config;
using authentication_engine.Features.Offices;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
{
    public class OfficeSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Offices.AnyAsync())
            {
                Logger.LogInformation("Seeding Offices data...");

                // Create a List instead of array
                var offices = new List<OfficeSeedDto>
                {
                    new OfficeSeedDto { Name = "TANAPA-HQ Arusha", Code = "HQ", ParentOffice = 0, StructureName = "Tanapa HQ" },
                    new OfficeSeedDto { Name = "Eastern Zone", Code = "EZ", ParentOffice = 1, StructureName = "Zone" },
                    new OfficeSeedDto { Name = "Western Zone", Code = "WZ", ParentOffice = 1, StructureName = "Zone" },
                    new OfficeSeedDto { Name = "Northern Zone", Code = "NZ", ParentOffice = 1, StructureName = "Zone" },
                    new OfficeSeedDto { Name = "Southern Zone", Code = "SZ", ParentOffice = 1, StructureName = "Zone" }
                };
                
                var allParks = await context.Parks.AsNoTracking().ToListAsync();
                
                // Add parks directly to the offices list
                foreach (var park in allParks)
                {
                    offices.Add(new OfficeSeedDto
                    { 
                        Name = park.Name,
                        Code = "",
                        ParentOffice = 2,
                        StructureName = "Parks",
                        ParkId = park.Id
                    });
                }
                
                foreach (var office in offices)
                {
                    var structure = await context.Structures.FirstOrDefaultAsync(m => m.Name == office.StructureName);
                    if (structure is null)
                        throw new KeyNotFoundException($"Structure not found for name '{office.StructureName}'.");
                    
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                    if (user is null)
                        throw new KeyNotFoundException($"User not found for name.");
                    
                    var entity = new Office
                    {
                        Id = Guid.NewGuid(),
                        Name = office.Name,
                        Code = office.Code,
                        ParentOffice = office.ParentOffice,
                        StructureId = structure.Id,
                        ParkId = office.ParkId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    };

                    await context.Offices.AddAsync(entity);
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}