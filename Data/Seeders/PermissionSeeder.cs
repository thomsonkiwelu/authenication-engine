using conservation_backend.Config;
using conservation_backend.Features.Permissions;
using conservation_backend.Features.SystemModules;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders
{
    public class PermissionSeeder : IBaseSeeder
    {
        public async Task SeedAsync(AppDBContext context)
        {
            
            Logger.LogInformation("Seeding Permission data ...");

            var seedUserId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                             ?? await context.Users.FirstOrDefaultAsync())
                ?.Id;

            var modelsWithModules = new Dictionary<string, string>
            {
                { "Vegetation", "ecology" },{ "Invasive_Species", "ecology" },{ "Road_Kill", "ecology" },{ "Weather", "ecology" },{ "Waste_Management", "ecology" },
                { "Rare_Endangered", "ecology" },{ "Ground_Count", "ecology" },{ "Aerial_Census", "ecology" },{ "Habitat_Manipulation", "ecology" },
                { "Migratory_Bird", "ecology" },{ "Water_Quality", "ecology" },{ "Water_Quantity", "ecology" },
                { "Fire_Prescription", "ecology" },{ "Fire_Break", "ecology" },{ "Wild_Fire", "ecology" },{ "Fire_Seminar", "ecology" },{ "Sighting_Turtle", "ecology" },
                { "Nesting_Turtle", "ecology" },{ "Death_Turtle", "ecology" },{ "Bird_Survey", "ecology" },{ "Bird_Line_Transect", "ecology" },{ "Mangabey_Observation", "ecology" },
                { "Mangabey_Monitoring", "ecology" },
                
                //Community permissions
                { "Cocoba", "community" },{ "Village_Profile", "community" },

                //LESS permissions
                { "Less_Operational_Zone", "less" },
                { "Less_Ranger_Station", "less" },
                { "Less_Ranger_Group", "less" },

                //LESS Configurations (Setting module)
                { "Less_Staff_Distribution_Config", "setting" },
                { "Less_Livestock_Config", "setting" },
                { "Less_Hwc_Config", "setting" },
                
                //Setting permissions
                { "User", "setting" },{"Permission", "setting"},{"Ranks", "setting"},{"Role", "setting"},{"Structure", "setting"},{"Staffs", "setting"},{"Locations", "setting"},
                {"Parks", "setting"},{"Waste_stations", "setting"},{"Species", "setting"},{"Less_activity", "setting"},{"Water_bodies", "setting"},{"Weather_stations", "setting"},
                
                //Dashboard permissions
                { "Less_Dashboard", "less" }, { "Setting_Dashboard", "setting" }, { "Ecology_Dashboard", "ecology" }
            };

            var actions = new[] { "view", "create", "update", "delete"};

            foreach (var (model, module) in modelsWithModules)
            {
                var modelName = model.ToLower();

                var systemModule = await context.SystemModules.FirstOrDefaultAsync(m => m.Slug == module);

                if (systemModule is null)
                {
                    var moduleName = module switch
                    {
                        "less" => "Law Enforcement & Security",
                        _ => module
                    };

                    systemModule = new SystemModule
                    {
                        Id = Guid.NewGuid(),
                        Name = moduleName,
                        Slug = module,
                        CreatedBy = seedUserId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    context.SystemModules.Add(systemModule);
                    await context.SaveChangesAsync();
                }

                foreach (var action in actions)
                {
                    var permissionName = $"{action}_{modelName}";
                    
                    var existingPermission = await context.Permissions
                        .FirstOrDefaultAsync(p => p.Name == permissionName);

                    if (existingPermission == null)
                    {
                        context.Permissions.Add(new PermissionEntity
                        {
                            Id = Guid.NewGuid(),
                            Name = permissionName,
                            Action = action,
                            SystemModuleId = systemModule.Id,
                            ModelType = model.Replace("_", " "),
                            CreatedBy = seedUserId,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
