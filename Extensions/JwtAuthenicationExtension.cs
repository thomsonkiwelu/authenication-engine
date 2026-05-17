using System.Text;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace authentication_engine.Extensions
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                var missing = new List<string>();
                if (string.IsNullOrWhiteSpace(secretKey)) missing.Add("JwtSettings:SecretKey");
                if (string.IsNullOrWhiteSpace(issuer)) missing.Add("JwtSettings:Issuer");
                if (string.IsNullOrWhiteSpace(audience)) missing.Add("JwtSettings:Audience");

                throw new InvalidOperationException(
                    $"JWT authentication is not configured. Missing configuration values: {string.Join(", ", missing)}. " +
                    "Configure them in appsettings or environment variables (e.g. JwtSettings__SecretKey)."
                );
            }

            var key = Encoding.UTF8.GetBytes(secretKey);
          
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            
                            throw new UnauthorizedAccessException(ResponseMessages.Unauthorized);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            
                            throw new UnauthorizedAccessException(ResponseMessages.Forbidden);
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
