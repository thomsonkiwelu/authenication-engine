using conservation_backend.Config;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Data.Seeders;

public class UgallaLessOperationalSeeder : IBaseSeeder
{
    private record ZoneSeedItem(string Name, string Code);

    private record StationSeedItem(string Name, string Code, string ZoneName);

    public async Task SeedAsync(AppDBContext context)
    {
        Logger.LogInformation("Seeding Ugalla LESS operational zones and stations ...");

        var seedUserId = (await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == "thomson.kiwelu")
                          ?? await context.Users.AsNoTracking().FirstOrDefaultAsync())
            ?.Id;

        if (seedUserId is null)
            return;

        var ugallaPark = await context.Parks.AsNoTracking().FirstOrDefaultAsync(p => p.Name == "Ugalla River National Park");
        if (ugallaPark is null)
            throw new KeyNotFoundException("Park not found: Ugalla River National Park");

        var now = DateTime.UtcNow;

        var zonesToSeed = new ZoneSeedItem[]
        {
            new("Ugalla_kati hq", "ugalla_kati"),
            new("Ugalla_mashariki", "Ugall_mash"),
            new("Ugalla_magharibi", "Ugalla_magh"),
        };

        var existingZones = await context.LessOperationalZones
            .Where(z => z.ParkId == ugallaPark.Id && z.DeletedAt == null)
            .ToListAsync();

        foreach (var z in zonesToSeed)
        {
            var existing = existingZones.FirstOrDefault(x => string.Equals(x.Name, z.Name, StringComparison.OrdinalIgnoreCase));
            if (existing is null)
            {
                var entity = new LessOperationalZone
                {
                    Id = Guid.NewGuid(),
                    Name = z.Name,
                    Code = z.Code,
                    ParkId = ugallaPark.Id,
                    CreatedAt = now,
                    CreatedBy = seedUserId,
                };

                await context.LessOperationalZones.AddAsync(entity);
                existingZones.Add(entity);
            }
            else if (!string.Equals(existing.Code ?? string.Empty, z.Code, StringComparison.Ordinal))
            {
                existing.Code = z.Code;
                existing.UpdatedAt = now;
                existing.UpdatedBy = seedUserId;
            }
        }

        await context.SaveChangesAsync();

        var zoneByName = existingZones
            .ToDictionary(z => z.Name.Trim().ToLowerInvariant(), z => z);

        var stationsToSeed = new StationSeedItem[]
        {
            new("Iluma hq", "iluma", "Ugalla_kati hq"),
            new("Lunyeta", "Lunyeta", "Ugalla_mashariki"),
            new("Lumbe", "Lumbe", "Ugalla_magharibi"),
        };

        var existingStations = await context.LessRangerStations
            .Include(s => s.LessOperationalZone)
            .Where(s => s.DeletedAt == null && s.LessOperationalZone.ParkId == ugallaPark.Id)
            .ToListAsync();

        foreach (var s in stationsToSeed)
        {
            if (!zoneByName.TryGetValue(s.ZoneName.Trim().ToLowerInvariant(), out var zone))
                throw new KeyNotFoundException($"LessOperationalZone not found for '{s.ZoneName}'");

            var existing = existingStations.FirstOrDefault(x =>
                x.LessOperationalZoneId == zone.Id &&
                string.Equals(x.Name, s.Name, StringComparison.OrdinalIgnoreCase));

            if (existing is null)
            {
                var entity = new LessRangerStation
                {
                    Id = Guid.NewGuid(),
                    Name = s.Name,
                    Code = s.Code,
                    LessOperationalZoneId = zone.Id,
                    CreatedAt = now,
                    CreatedBy = seedUserId,
                };

                await context.LessRangerStations.AddAsync(entity);
                existingStations.Add(entity);
            }
            else if (!string.Equals(existing.Code ?? string.Empty, s.Code, StringComparison.Ordinal))
            {
                existing.Code = s.Code;
                existing.UpdatedAt = now;
                existing.UpdatedBy = seedUserId;
            }
        }

        await context.SaveChangesAsync();
    }
}
