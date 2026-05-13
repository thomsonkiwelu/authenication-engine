using System.Reflection;
using Microsoft.OpenApi.Models;

namespace authentication_engine.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Conservation Backend API",
                    Description = "API for Conservation Management System",
                    Contact = new OpenApiContact
                    {
                        Name = "API Support",
                        Email = "support@conservation.com"
                    },
                });

                // Add JWT Authentication
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
                                    "Enter 'Bearer' [space] and then your token.\r\n\r\n" +
                                    "Example: \"Bearer 12345abcdef\""
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                // Custom schema IDs
                //options.CustomSchemaIds(x => x.FullName);

            });

            return services;
        }
    }
}
