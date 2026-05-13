using conservation_backend.Config;

namespace conservation_backend.Data.Seeders
{
    public interface IBaseSeeder
    {
        Task SeedAsync(AppDBContext context);
    }

}
