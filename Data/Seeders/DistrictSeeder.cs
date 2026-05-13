using conservation_backend.Config;
using conservation_backend.Features.Districts;
using conservation_backend.Features.Regions;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class DistrictSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            if (!await context.Districts.AnyAsync())
            {
                Logger.LogInformation("Seeding Districts data ...");
                
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu");
                if (user is null)
                    throw new KeyNotFoundException($"User not found.");
                
                var districts = new[]
                {
                    //Arusha
                    new { Name = "Arusha CBD", Region = "Arusha" },
                    new { Name = "Arusha", Region = "Arusha" },
                    new { Name = "Arumeru", Region = "Arusha" },
                    new { Name = "Karatu", Region = "Arusha" },
                    new { Name = "Monduli", Region = "Arusha" },
                    new { Name = "Longido", Region = "Arusha" },
                    new { Name = "Ngorongoro", Region = "Arusha" },
                    //Dar es Salaam
                    new { Name = "Ilala", Region = "Dar es Salaam" },
                    new { Name = "Ilala CBD", Region = "Dar es Salaam" },
                    new { Name = "Kigamboni", Region = "Dar es Salaam" },
                    new { Name = "Kinondoni", Region = "Dar es Salaam" },
                    new { Name = "Temeke", Region = "Dar es Salaam" },
                    new { Name = "Ubungo", Region = "Dar es Salaam" },
                    //Dodoma
                    new { Name = "Bahi", Region = "Dodoma" },
                    new { Name = "Chamwino", Region = "Dodoma" },
                    new { Name = "Chemba", Region = "Dodoma" },
                    new { Name = "Dodoma", Region = "Dodoma" },
                    new { Name = "Dodoma CBD", Region = "Dodoma" },
                    new { Name = "Kondoa", Region = "Dodoma" },
                    new { Name = "Kongwa", Region = "Dodoma" },
                    new { Name = "Mpwapwa", Region = "Dodoma" },
                    //Geita
                    new { Name = "Bukombe", Region = "Geita" },
                    new { Name = "Chato", Region = "Geita" },
                    new { Name = "Geita", Region = "Geita" },
                    new { Name = "Mbogwe", Region = "Geita" },
                    new { Name = "Nyang'hwale", Region = "Geita" },
                    //Iringa
                    new { Name = "Iringa", Region = "Iringa" },
                    new { Name = "Iringa CBD", Region = "Iringa" },
                    new { Name = "Kilolo", Region = "Iringa" },
                    new { Name = "Mufindi", Region = "Iringa" },
                    //Kagera
                    new { Name = "Biharamulo", Region = "Kagera" },
                    new { Name = "Bukoba", Region = "Kagera" },
                    new { Name = "Bukoba CBD", Region = "Kagera" },
                    new { Name = "Karagwe", Region = "Kagera" },
                    new { Name = "Kyerwa", Region = "Kagera" },
                    new { Name = "Missenyi", Region = "Kagera" },
                    new { Name = "Muleba", Region = "Kagera" },
                    new { Name = "Ngara", Region = "Kagera" },
                    //Katavi
                    new { Name = "Mlele", Region = "Katavi" },
                    new { Name = "Mpanda CBD", Region = "Katavi" },
                    new { Name = "Tanganyika", Region = "Katavi" },
                    //Mwanza
                    new { Name = "Ilemela", Region = "Mwanza" },
                    new { Name = "Kwimba", Region = "Mwanza" },
                    new { Name = "Magu", Region = "Mwanza" },
                    new { Name = "Misungwi", Region = "Mwanza" },
                    new { Name = "Nyamagana", Region = "Mwanza" },
                    new { Name = "Sengerema", Region = "Mwanza" },
                    new { Name = "Ukerewe", Region = "Mwanza" },
                    //Kilimanjaro
                    new { Name = "Moshi CBD", Region = "Kilimanjaro" },
                    new { Name = "Moshi", Region = "Kilimanjaro" },
                    new { Name = "Rombo", Region = "Kilimanjaro" },
                    new { Name = "Same", Region = "Kilimanjaro" },
                    new { Name = "Mwanga", Region = "Kilimanjaro" },
                    new { Name = "Hai", Region = "Kilimanjaro" },
                    new { Name = "Siha", Region = "Kilimanjaro" },
                    //Manyara
                    new { Name = "Babati", Region = "Manyara" },
                    new { Name = "Babati CBD", Region = "Manyara" },
                    new { Name = "Hanang", Region = "Manyara" },
                    new { Name = "Kiteto", Region = "Manyara" },
                    new { Name = "Mbulu", Region = "Manyara" },
                    new { Name = "Simanjiro", Region = "Manyara" },
                    //Tanga
                    new { Name = "Tanga", Region = "Tanga" },
                    new { Name = "Muheza", Region = "Tanga" },
                    new { Name = "Korogwe", Region = "Tanga" },
                    new { Name = "Pangani", Region = "Tanga" },
                    new { Name = "Handeni", Region = "Tanga" },
                    new { Name = "Lushoto", Region = "Tanga" },
                    new { Name = "Mkinga", Region = "Tanga" },
                    new { Name = "Kilindi", Region = "Tanga" },
                    //Mara
                    new { Name = "Bunda", Region = "Mara" },
                    new { Name = "Butiama", Region = "Mara" },
                    new { Name = "Musoma CBD", Region = "Mara" },
                    new { Name = "Rorya", Region = "Mara" },
                    new { Name = "Serengeti", Region = "Mara" },
                    new { Name = "Tarime", Region = "Mara" },
                    //Shinyanga
                    new { Name = "Kahama", Region = "Shinyanga" },
                    new { Name = "Kishapu", Region = "Shinyanga" },
                    new { Name = "Shinyanga", Region = "Shinyanga" },
                    new { Name = "Shinyanga CBD", Region = "Shinyanga" },
                    //Simiyu
                    new { Name = "Bariadi", Region = "Simiyu" },
                    new { Name = "Busega", Region = "Simiyu" },
                    new { Name = "Itilima", Region = "Simiyu" },
                    new { Name = "Maswa", Region = "Simiyu" },
                    new { Name = "Meatu", Region = "Simiyu" },
                    //Singida
                    new { Name = "Ikungi", Region = "Singida" },
                    new { Name = "Iramba", Region = "Singida" },
                    new { Name = "Manyoni", Region = "Singida" },
                    new { Name = "Mkalama", Region = "Singida" },
                    new { Name = "Singida", Region = "Singida" },
                    new { Name = "Singida CBD", Region = "Singida" },
                    //Tabora
                    new { Name = "Igunga", Region = "Tabora" },
                    new { Name = "Kaliua", Region = "Tabora" },
                    new { Name = "Nzega", Region = "Tabora" },
                    new { Name = "Sikonge", Region = "Tabora" },
                    new { Name = "Tabora CBD", Region = "Tabora" },
                    new { Name = "Urambo", Region = "Tabora" },
                    new { Name = "Uyui", Region = "Tabora" },
                    //Pwani
                    new { Name = "Bagamoyo", Region = "Pwani" },
                    new { Name = "Kibaha", Region = "Pwani" },
                    new { Name = "Kibaha CBD", Region = "Pwani" },
                    new { Name = "Kibiti", Region = "Pwani" },
                    new { Name = "Kisarawe", Region = "Pwani" },
                    new { Name = "Mafia", Region = "Pwani" },
                    new { Name = "Mkuranga", Region = "Pwani" },
                    new { Name = "Rufiji", Region = "Pwani" },
                    //Morogoro
                    new { Name = "Gairo", Region = "Morogoro" },
                    new { Name = "Kilombero", Region = "Morogoro" },
                    new { Name = "Kilosa", Region = "Morogoro" },
                    new { Name = "Malinyi", Region = "Morogoro" },
                    new { Name = "Morogoro", Region = "Morogoro" },
                    new { Name = "Morogoro CBD", Region = "Morogoro" },
                    new { Name = "Mvomero", Region = "Morogoro" },
                    new { Name = "Ulanga", Region = "Morogoro" },
                    //Lindi
                    new { Name = "Kilwa", Region = "Lindi" },
                    new { Name = "Lindi", Region = "Lindi" },
                    new { Name = "Lindi CBD", Region = "Lindi" },
                    new { Name = "Liwale", Region = "Lindi" },
                    new { Name = "Nachingwea", Region = "Lindi" },
                    new { Name = "Ruangwa", Region = "Lindi" },
                    //Mtwara
                    new { Name = "Masasi", Region = "Mtwara" },
                    new { Name = "Mtwara", Region = "Mtwara" },
                    new { Name = "Mtwara CBD", Region = "Mtwara" },
                    new { Name = "Nanyumbu", Region = "Mtwara" },
                    new { Name = "Newala", Region = "Mtwara" },
                    new { Name = "Tandahimba", Region = "Mtwara" },
                    //Mbeya
                    new { Name = "Chunya", Region = "Mbeya" },
                    new { Name = "Kyela", Region = "Mbeya" },
                    new { Name = "Mbarali", Region = "Mbeya" },
                    new { Name = "Mbeya", Region = "Mbeya" },
                    new { Name = "Mbeya CBD", Region = "Mbeya" },
                    new { Name = "Rungwe", Region = "Mbeya" },
                    //Njombe
                    new { Name = "Ludewa", Region = "Njombe" },
                    new { Name = "Makete", Region = "Njombe" },
                    new { Name = "Njombe", Region = "Njombe" },
                    new { Name = "Njombe CBD", Region = "Njombe" },
                    new { Name = "Wanging'ombe", Region = "Njombe" },
                    //Rukwa
                    new { Name = "Kalambo", Region = "Rukwa" },
                    new { Name = "Nkasi", Region = "Rukwa" },
                    new { Name = "Sumbawanga", Region = "Rukwa" },
                    new { Name = "Sumbawanga CBD", Region = "Rukwa" },
                    //Songwe
                    new { Name = "Ileje", Region = "Songwe" },
                    new { Name = "Mbozi", Region = "Songwe" },
                    new { Name = "Momba", Region = "Songwe" },
                    new { Name = "Songwe", Region = "Songwe" },
                    //Kigoma
                    new { Name = "Buhigwe", Region = "Kigoma" },
                    new { Name = "Kakonko", Region = "Kigoma" },
                    new { Name = "Kasulu", Region = "Kigoma" },
                    new { Name = "Kibondo", Region = "Kigoma" },
                    new { Name = "Kigoma", Region = "Kigoma" },
                    new { Name = "Kigoma CBD", Region = "Kigoma" },
                    new { Name = "Uvinza", Region = "Kigoma" },
                    //Ruvuma
                    new { Name = "Mbinga", Region = "Ruvuma" },
                    new { Name = "Namtumbo", Region = "Ruvuma" },
                    new { Name = "Nyasa", Region = "Ruvuma" },
                    new { Name = "Songea", Region = "Ruvuma" },
                    new { Name = "Songea CBD", Region = "Ruvuma" },
                    new { Name = "Tunduru", Region = "Ruvuma" }
                };
                
                foreach (var district in districts)
                {
                    var region = await context.Regions.FirstOrDefaultAsync(u => u.Name == district.Region);
                    if (region is null)
                        throw new KeyNotFoundException($"Region not found. {district.Region}");
                    
                    await context.Districts.AddAsync(new District
                    {
                        Name = district.Name,
                        RegionId = region.Id,
                        CreatedBy = user.Id,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
