using conservation_backend.Config;
using conservation_backend.Data.Seeders;
using conservation_backend.Shared;

namespace conservation_backend.Extensions
{
    public static class SeederExtension
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                try{
                    Logger.LogInformation("The seeder extension is currently running...");

                    var context = services.GetRequiredService<AppDBContext>();

                    //Get all registered seeders
                    var seeders = scope.ServiceProvider.GetServices<IBaseSeeder>();

                    // Now run the seeders
                    foreach (var seeder in seeders)
                    {
                        await seeder.SeedAsync(context);
                    }

                    Logger.LogInformation("The seeder extension has completed successfully");

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred while seeding data into database.");
                }
            }
        }
    }
}
