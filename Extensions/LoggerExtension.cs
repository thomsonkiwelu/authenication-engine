using authentication_engine.Shared;

namespace authentication_engine.Extensions
{
    public static class LoggerExtension
    {
        public static void UseAppLogger(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var mainLogger = serviceProvider.GetRequiredService<ILogger<Program>>();

                Logger.Initialize(mainLogger);
            }
        }
    }
}
