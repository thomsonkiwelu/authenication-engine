using authentication_engine.Config;
using authentication_engine.Extensions;
using authentication_engine.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalValidationFilter>();
});

// Add Mapster for mapping dtos
builder.Services.AddMapster();

// Add database config
builder.Services.AddDbContext<AppDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "Database connection string is missing. Configure ConnectionStrings:DefaultConnection (e.g. env var ConnectionStrings__DefaultConnection)."
        );
    }

    options.UseNpgsql(
        connectionString
     );

    if (builder.Environment.IsDevelopment())
    {
        options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
});

// Add Authentication & Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCustomSwaggerGen(builder.Configuration);

//Add service classes
builder.Services.AddClassScope();

//Add global httpContext access
builder.Services.AddHttpContextAccessor();

// Add Cross Origin
builder.Services.AddCorsPolicy();

//Add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Global exception handler
app.UseExceptionHandler(_ => { });

// Application logger
app.UseAppLogger();

var runMigrate = args.Contains("--migrate") || args.Contains("--migrate-and-seed");
var runSeed = args.Contains("--seed") || args.Contains("--migrate-and-seed");

if (runMigrate)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    await context.Database.MigrateAsync();
}

if (runSeed)
{
    await app.SeedDataAsync();
}

if (runMigrate || runSeed)
{
    return;
}

// Apply seed data on startup
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    await context.Database.MigrateAsync();
    await app.SeedDataAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Serve static files
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
