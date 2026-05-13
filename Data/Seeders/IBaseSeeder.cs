using authentication_engine.Config;

namespace authentication_engine.Data.Seeders
{
    public interface IBaseSeeder
    {
        Task SeedAsync(AppDBContext context);
    }

}
