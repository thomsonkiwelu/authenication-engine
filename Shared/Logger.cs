namespace authentication_engine.Shared
{
    public static class Logger
    {
        private static ILogger? _logger;

        public static void Initialize(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null during initialization.");
        }

        public static void LogInformation(string message, params object[] args)
        {
            _logger?.LogInformation(message, args);
        }

        public static void LogWarning(string message, params object[] args)
        {
            _logger?.LogWarning(message, args);
        }

        public static void LogError(string message, params object[] args)
        {
            _logger?.LogError(message, args);
        }

        public static void LogError(Exception exception, string message, params object[] args)
        {
            _logger?.LogError(exception, message, args);
        }
    }

}
