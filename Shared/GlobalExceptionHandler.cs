using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace authentication_engine.Shared
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            var env = httpContext.RequestServices.GetService<IHostEnvironment>();
            
            var (statusCode, title) = MapException(exception);

            Logger.LogError(exception, "{Message} | traceId: {TraceId}", exception.Message, traceId);

            await Results.Problem(
                title: title,
                //detail: env?.IsDevelopment() == true ? exception.ToString() : null,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                {
                    {"traceId", traceId}
                }
            ).ExecuteAsync(httpContext);

            return true;
        }

        private static (int StatusCode, string Title) MapException(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound, exception.Message),

                ArgumentNullException =>
                    (StatusCodes.Status400BadRequest, exception.Message),

                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, exception.Message),

                SecurityTokenException =>
                    (StatusCodes.Status401Unauthorized, exception.Message),

                PostgresException => 
                    (StatusCodes.Status500InternalServerError, ResponseMessages.DatabaseError),
                
                BadHttpRequestException =>
                    (StatusCodes.Status400BadRequest, exception.Message),

                _ => (StatusCodes.Status500InternalServerError, ResponseMessages.Error)
            };
        }
    }
}
