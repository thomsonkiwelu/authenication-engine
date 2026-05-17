using authentication_engine.Config;
using authentication_engine.Features.Permissions;
using authentication_engine.Features.SystemModules;
using authentication_engine.Shared;
using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Data.Seeders
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
                { "Less_Operational_Zone", "law_enforcement_security" },
                { "Less_Ranger_Station", "law_enforcement_security" },
                { "Less_Ranger_Group", "law_enforcement_security" },

                //LESS Configurations (Setting module)
                { "Less_Staff_Distribution_Config", "setting" },
                { "Less_Livestock_Config", "setting" },
                { "Less_Hwc_Config", "setting" },
                
                //Setting in conservation system
                {"Locations", "setting"},{"Parks", "setting"},{"Waste_stations", "setting"},{"Species", "setting"},{"Less_activity", "setting"},{"Water_bodies", "setting"},
                {"Weather_stations", "setting"},
                
                //User Management system
                { "User", "user_management" },{"Permission", "user_management"},{"Ranks", "user_management"},{"Role", "user_management"},{"Structure", "user_management"},{"Staffs", "user_management"},
                {"Application", "user_management"},{"System_Module", "user_management"},
                
                //Dashboard permissions
                { "Less_Dashboard", "law_enforcement_security" }, { "Setting_Dashboard", "setting" }, { "Ecology_Dashboard", "ecology" }
            };

            var actions = new[] { "view", "create", "update", "delete"};

            foreach (var (model, module) in modelsWithModules)
            {
                var modelName = model.ToLower();

                var systemModule = await context.SystemModules.FirstOrDefaultAsync(m => m.Slug == module);

                if (systemModule is null)
                    throw new KeyNotFoundException($"System module not found.");

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
                            SystemApplicationId =  systemModule.SystemApplicationId,
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
