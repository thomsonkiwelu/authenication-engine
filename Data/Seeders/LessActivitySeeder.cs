using conservation_backend.Config;
using conservation_backend.Features.LessActivities;
using conservation_backend.Features.LessRangerDivisionConfig;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders;

public class LessActivitySeeder : IBaseSeeder
{
    public async Task SeedAsync(AppDBContext context)
    {
        Logger.LogInformation("Seeding Less activities ...");

        var seedUserId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                          ?? await context.Users.FirstOrDefaultAsync())
            ?.Id;

        if (seedUserId == null)
            return;

        var hasAnyFields = await context.LessActivities.AnyAsync();
        if (hasAnyFields)
            return;

        var now = DateTime.UtcNow;

        var officerFields = new (string Label, string Key)[]
        {
            ("Maafisa waliopo Doria", "maafisaWaliopodoria"),
            ("Mawasiliano / Radio room", "radioRoom"),
            ("Utunzaji Ghala la Silaha", "utunzajiSilaha"),
            ("Safari kikazi", "safarikikazi"),
            ("Kazi za ofisini", "kaziOfisini"),
            ("Uokoaji Mlimani / Rescue", "uokoajiRescue"),
            ("Mafunzo/Masomoni", "mafunzoMasomoni"),
            ("Kazi maalumu", "kaziMaalumu"),
            ("Shughuli za Mahakamani", "kuhudhuriaMahakamani"),
            ("Shughuli za Utalii", "shughulizautalii"),
            ("Idadi ya wagonjwa", "idadiYaWagonjwa"),
            ("Idadi ya ruhusa", "idadiYaRuhusa"),
            ("Idadi ya likizo", "idadiYaLikizo"),
            ("Idadi ya mapumziko", "idadiYaMapumziko"),
        };

        var rangerFields = new (string Label, string Key)[]
        {
            ("Askari waliopo doria", "askariWaliopoDoria"),
            ("Askari waliopo standby", "askariWaliopoStandby"),
            ("Idadi ya walinzi", "idadiYaWalinzi"),
            ("Idadi ya Escort", "idadiYaEscort"),
            ("Askari waongoza wageni", "askariWaongozaWageni"),
            ("Utunzaji Ghala la Silaha", "utunzajiSilaha"),
            ("Mawasiliano / Radio room", "radioRoom"),
            ("Kazi za ofisini", "kaziOfisini"),
            ("Safari kikazi", "safarikikazi"),
            ("Uokoaji Mlimani / Rescue", "uokoajiRescue"),
            ("Mafunzo/Masomoni", "mafunzoMasomoni"),
            ("Kazi maalumu", "kaziMaalumu"),
            ("Shughuli za Mahakamani", "kuhudhuriaMahakamani"),
            ("Shughuli za Utalii", "shughulizautalii"),
            ("Idadi ya wagonjwa", "idadiYaWagonjwa"),
            ("Idadi ya ruhusa", "idadiYaRuhusa"),
            ("Idadi ya likizo", "idadiYaLikizo"),
            ("Idadi ya mapumziko", "idadiYaMapumziko"),
        };

        var toInsert = new List<LessActivity>();

        var officerOrder = 1;
        foreach (var f in officerFields)
        {
            toInsert.Add(new LessActivity
            {
                Category = "officer",
                Label = f.Label,
                Key = f.Key,
                SortOrder = officerOrder++,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            });
        }

        var rangerOrder = 1;
        foreach (var f in rangerFields)
        {
            toInsert.Add(new LessActivity
            {
                Category = "ranger",
                Label = f.Label,
                Key = f.Key,
                SortOrder = rangerOrder++,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            });
        }

        context.LessActivities.AddRange(toInsert);
        await context.SaveChangesAsync();
    }
}
