using conservation_backend.Config;
using conservation_backend.Features.LessHwcConfig;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders;

public class LessHwcConfigSeeder : IBaseSeeder
{
    public async Task SeedAsync(AppDBContext context)
    {
        Logger.LogInformation("Seeding LESS HWC configuration ...");

        var seedUserId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                          ?? await context.Users.FirstOrDefaultAsync())
            ?.Id;

        if (seedUserId == null)
            return;

        var hasAny = await context.LessHwcFieldDefinitions.AnyAsync() || await context.LessHwcTabDefinitions.AnyAsync();
        if (hasAny)
            return;

        var now = DateTime.UtcNow;

        var tabs = new List<LessHwcTabDefinition>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Key = "location",
                Label = "Location",
                SortOrder = 1,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "incidents",
                Label = "Incident Counts",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "response",
                Label = "Response",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Key = "followup",
                Label = "Follow-up",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
        };

        context.LessHwcTabDefinitions.AddRange(tabs);

        Guid Tab(string key) => tabs.First(t => t.Key == key).Id;

        var fields = new List<LessHwcFieldDefinition>
        {
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "tarehe",
                Label = "Date",
                FieldType = "date",
                IsRequired = true,
                SortOrder = 1,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "wilaya",
                Label = "District",
                FieldType = "text",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "kijiji",
                Label = "Village(s)",
                FieldType = "text",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "kata",
                Label = "Ward",
                FieldType = "text",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "gpsLatitude",
                Label = "GPS Latitude",
                FieldType = "number",
                SortOrder = 5,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "gpsLongitude",
                Label = "GPS Longitude",
                FieldType = "number",
                SortOrder = 6,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("location"),
                Key = "ainaYaUharibifu",
                Label = "Incident Category",
                FieldType = "select",
                IsRequired = true,
                SortOrder = 7,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },

            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "uharibifuWaMakazi",
                Label = "Damage to Houses/Structures (count)",
                FieldType = "number",
                SortOrder = 1,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "uharibifuWaMazao",
                Label = "Crop Damage (count)",
                FieldType = "number",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "vifoVyaWatu",
                Label = "Human Fatalities (count)",
                FieldType = "number",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "watuWaliojeruhiwa",
                Label = "Human Injuries (count)",
                FieldType = "number",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "tishio",
                Label = "Threat/Attempted Incident (count)",
                FieldType = "number",
                SortOrder = 5,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "matukioYaWanyamaWalioliwa",
                Label = "Livestock Predation Incidents (count)",
                FieldType = "number",
                SortOrder = 6,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "kulipizaKisasi",
                Label = "Retaliation Incidents (count)",
                FieldType = "number",
                SortOrder = 7,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("incidents"),
                Key = "jumla",
                Label = "Total Incidents (auto/optional)",
                FieldType = "number",
                IsComputed = true,
                ComputeFromKeys = "uharibifuWaMakazi,uharibifuWaMazao,vifoVyaWatu,watuWaliojeruhiwa,tishio,matukioYaWanyamaWalioliwa,kulipizaKisasi",
                SortOrder = 8,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },

            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("response"),
                Key = "idadiYaMatukioYaliyoripotiwa",
                Label = "Incidents Reported (count)",
                FieldType = "number",
                SortOrder = 1,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("response"),
                Key = "idadiYaMatukioYaliyofanyiwaKazi",
                Label = "Incidents Responded To (count)",
                FieldType = "number",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("response"),
                Key = "wanyamaWaharibifu",
                Label = "Problem Animal Species",
                FieldType = "select",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("response"),
                Key = "taasisiZilizoshiriki",
                Label = "Participating Institutions",
                FieldType = "multiselect",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("response"),
                Key = "idadiYaWashirikiWaDoria",
                Label = "Number of Participants",
                FieldType = "number",
                SortOrder = 5,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("response"),
                Key = "hatuaIliyofanyika",
                Label = "Action Taken",
                FieldType = "textarea",
                SortOrder = 6,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },

            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("followup"),
                Key = "ufuatiliajiUnahitajika",
                Label = "Follow-up Required",
                FieldType = "select",
                SortOrder = 1,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("followup"),
                Key = "makadirioYaHasara",
                Label = "Estimated Loss (TZS)",
                FieldType = "number",
                SortOrder = 2,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("followup"),
                Key = "maelezo",
                Label = "Description/Remarks",
                FieldType = "textarea",
                SortOrder = 3,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
            new()
            {
                Id = Guid.NewGuid(),
                TabDefinitionId = Tab("followup"),
                Key = "kumbukumbuNa",
                Label = "Reference No.",
                FieldType = "text",
                SortOrder = 4,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            },
        };

        context.LessHwcFieldDefinitions.AddRange(fields);

        LessHwcFieldDefinition F(string key) => fields.First(f => f.Key == key);

        var options = new List<LessHwcFieldOption>();

        var incidentCategories = new[]
        {
            "Property damage",
            "Crop damage",
            "Human death",
            "Human injury",
            "Threat",
            "Livestock predation",
            "Retaliation",
        };

        var order = 1;
        foreach (var opt in incidentCategories)
        {
            options.Add(new LessHwcFieldOption
            {
                Id = Guid.NewGuid(),
                FieldDefinitionId = F("ainaYaUharibifu").Id,
                Value = opt,
                Label = opt,
                SortOrder = order++,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            });
        }

        var animals = new[]
        {
            "Elephant",
            "Buffalo",
            "Lion",
            "Leopard",
            "Hyena",
            "Hippo",
            "Crocodile",
            "Baboon",
            "Monkey",
            "Warthog",
            "Other",
        };

        order = 1;
        foreach (var opt in animals)
        {
            options.Add(new LessHwcFieldOption
            {
                Id = Guid.NewGuid(),
                FieldDefinitionId = F("wanyamaWaharibifu").Id,
                Value = opt,
                Label = opt,
                SortOrder = order++,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            });
        }

        var institutions = new[]
        {
            "TANAPA",
            "TAWA",
            "Police",
            "Village Council",
            "District Council",
            "Community Scouts",
            "Other",
        };

        order = 1;
        foreach (var opt in institutions)
        {
            options.Add(new LessHwcFieldOption
            {
                Id = Guid.NewGuid(),
                FieldDefinitionId = F("taasisiZilizoshiriki").Id,
                Value = opt,
                Label = opt,
                SortOrder = order++,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            });
        }

        var yesNo = new[] { "Yes", "No" };
        order = 1;
        foreach (var opt in yesNo)
        {
            options.Add(new LessHwcFieldOption
            {
                Id = Guid.NewGuid(),
                FieldDefinitionId = F("ufuatiliajiUnahitajika").Id,
                Value = opt,
                Label = opt,
                SortOrder = order++,
                IsActive = true,
                CreatedAt = now,
                CreatedBy = seedUserId,
            });
        }

        context.LessHwcFieldOptions.AddRange(options);

        await context.SaveChangesAsync();
    }
}
