using conservation_backend.Config;
using conservation_backend.Features.LessPatrols.Interfaces;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessPatrols;

public class LessPatrolDailyService(
    AppDBContext context,
    IUserContext userContext,
    ILessPatrolDailyRepository repository
) : ILessPatrolDailyService
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    private readonly ILessPatrolDailyRepository _repository = repository;

    private record LessScope(Guid? ParkId, Guid? OfficeId);

    public async Task<LessPatrolDailyResponseDto> GetEntry(LessPatrolDailyGetRequest request)
    {
        var scope = await GetScopeOrThrow();

        if (!Guid.TryParse(request.StationId, out var stationId))
            throw new InvalidOperationException("Invalid station id");

        var dutyDate = ParseDateOnlyOrThrow(request.DutyDate);

        var station = await _context.LessRangerStations
            .AsNoTracking()
            .Include(s => s.LessOperationalZone)
            .Include(s => s.Office)
            .FirstOrDefaultAsync(s => s.Id == stationId);

        if (station is null)
            throw new KeyNotFoundException("Station not found");

        if (scope.ParkId.HasValue)
        {
            if (station.LessOperationalZone is null)
                throw new UnauthorizedAccessException("You are not authorized to access this station");

            if (station.LessOperationalZone.ParkId != scope.ParkId.Value)
                throw new UnauthorizedAccessException("You are not authorized to access this station");
        }
        else
        {
            if (!scope.OfficeId.HasValue)
                throw new UnauthorizedAccessException("Office is required for office-scoped users");

            if (station.OfficeId != scope.OfficeId.Value)
                throw new UnauthorizedAccessException("You are not authorized to access this station");
        }

        var existing = await _context.LessPatrolDailies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate);

        return new LessPatrolDailyResponseDto
        {
            PatrolDailyId = existing?.Id,
            ParkId = scope.ParkId,
            OfficeId = scope.OfficeId,
            LessOperationalZoneId = station.LessOperationalZoneId,
            LessOperationalZoneName = station.LessOperationalZone?.Name,
            StationId = stationId,
            StationName = station.Name,
            DutyDate = dutyDate,
            ManDaysPlanned = existing?.ManDaysPlanned ?? 0,
            ManDaysPerformed = existing?.ManDaysPerformed ?? 0,
            FootPatrolPlanned = existing?.FootPatrolPlanned ?? 0,
            FootPatrolPerformed = existing?.FootPatrolPerformed ?? 0,
            VehiclePatrolPlanned = existing?.VehiclePatrolPlanned ?? 0,
            VehiclePatrolPerformed = existing?.VehiclePatrolPerformed ?? 0,
            BoatPatrolPlanned = existing?.BoatPatrolPlanned ?? 0,
            BoatPatrolPerformed = existing?.BoatPatrolPerformed ?? 0,
            AirPatrolPlanned = existing?.AirPatrolPlanned ?? 0,
            AirPatrolPerformed = existing?.AirPatrolPerformed ?? 0,
            AirPatrolHoursPlanned = existing?.AirPatrolHoursPlanned ?? 0,
            AirPatrolHoursPerformed = existing?.AirPatrolHoursPerformed ?? 0,
            AreaInspectedKm = existing?.AreaInspectedKm ?? 0,
        };
    }

    public async Task<LessPatrolDailyResponseDto> SaveEntry(LessPatrolDailySaveRequest request)
    {
        var scope = await GetScopeOrThrow();

        if (!Guid.TryParse(request.StationId, out var stationId))
            throw new InvalidOperationException("Invalid station id");

        var dutyDate = ParseDateOnlyOrThrow(request.DutyDate);

        var station = await _context.LessRangerStations
            .AsNoTracking()
            .Include(s => s.LessOperationalZone)
            .Include(s => s.Office)
            .FirstOrDefaultAsync(s => s.Id == stationId);

        if (station is null)
            throw new KeyNotFoundException("Station not found");

        if (scope.ParkId.HasValue)
        {
            if (station.LessOperationalZone is null)
                throw new UnauthorizedAccessException("You are not authorized to access this station");

            if (station.LessOperationalZone.ParkId != scope.ParkId.Value)
                throw new UnauthorizedAccessException("You are not authorized to access this station");
        }
        else
        {
            if (!scope.OfficeId.HasValue)
                throw new UnauthorizedAccessException("Office is required for office-scoped users");

            if (station.OfficeId != scope.OfficeId.Value)
                throw new UnauthorizedAccessException("You are not authorized to access this station");
        }

        ValidateNonNegative(request);

        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var existing = await _context.LessPatrolDailies
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate);

        if (existing == null)
        {
            existing = new LessPatrolDaily
            {
                Id = Guid.NewGuid(),
                ParkId = scope.ParkId,
                OfficeId = scope.OfficeId,
                LessRangerStationId = stationId,
                DutyDate = dutyDate,
                CreatedAt = now,
                CreatedBy = userId,
            };

            _context.LessPatrolDailies.Add(existing);
        }
        else
        {
            existing.UpdatedAt = now;
            existing.UpdatedBy = userId;
        }

        existing.ManDaysPlanned = request.ManDaysPlanned;
        existing.ManDaysPerformed = request.ManDaysPerformed;
        existing.FootPatrolPlanned = request.FootPatrolPlanned;
        existing.FootPatrolPerformed = request.FootPatrolPerformed;
        existing.VehiclePatrolPlanned = request.VehiclePatrolPlanned;
        existing.VehiclePatrolPerformed = request.VehiclePatrolPerformed;
        existing.BoatPatrolPlanned = request.BoatPatrolPlanned;
        existing.BoatPatrolPerformed = request.BoatPatrolPerformed;
        existing.AirPatrolPlanned = request.AirPatrolPlanned;
        existing.AirPatrolPerformed = request.AirPatrolPerformed;
        existing.AirPatrolHoursPlanned = request.AirPatrolHoursPlanned;
        existing.AirPatrolHoursPerformed = request.AirPatrolHoursPerformed;
        existing.AreaInspectedKm = request.AreaInspectedKm;

        await _repository.SaveChanges();

        return await GetEntry(new LessPatrolDailyGetRequest
        {
            StationId = stationId.ToString(),
            DutyDate = dutyDate.ToString("yyyy-MM-dd"),
        });
    }

    public async Task<PagedList<LessPatrolDailyHeaderDto>> GetHeaders(LessPatrolDailyHeadersRequest request)
    {
        var scope = await GetScopeOrThrow();

        DateOnly? fromDate = null;
        DateOnly? toDate = null;

        if (!string.IsNullOrWhiteSpace(request.FromDate))
        {
            if (!DateOnly.TryParse(request.FromDate, out var parsed))
                throw new InvalidOperationException("Invalid fromDate format. Expected yyyy-MM-dd");
            fromDate = parsed;
        }

        if (!string.IsNullOrWhiteSpace(request.ToDate))
        {
            if (!DateOnly.TryParse(request.ToDate, out var parsed))
                throw new InvalidOperationException("Invalid toDate format. Expected yyyy-MM-dd");
            toDate = parsed;
        }

        Guid? zoneId = null;
        if (!string.IsNullOrWhiteSpace(request.LessOperationalZoneId))
        {
            if (!Guid.TryParse(request.LessOperationalZoneId, out var parsed))
                throw new InvalidOperationException("Invalid operational zone id");
            zoneId = parsed;
        }

        Guid? stationId = null;
        if (!string.IsNullOrWhiteSpace(request.StationId))
        {
            if (!Guid.TryParse(request.StationId, out var parsed))
                throw new InvalidOperationException("Invalid station id");
            stationId = parsed;
        }

        var baseQuery =
            from p in _context.LessPatrolDailies.AsNoTracking()
            join s in _context.LessRangerStations.AsNoTracking() on p.LessRangerStationId equals s.Id
            join z0 in _context.LessOperationalZones.AsNoTracking() on s.LessOperationalZoneId equals z0.Id into zg
            from z in zg.DefaultIfEmpty()
            where (scope.ParkId.HasValue && p.ParkId == scope.ParkId.Value)
                  || (scope.OfficeId.HasValue && p.OfficeId == scope.OfficeId.Value)
            select new
            {
                Patrol = p,
                Station = s,
                Zone = z,
            };

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Patrol.DutyDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Patrol.DutyDate <= toDate.Value);

        if (zoneId.HasValue)
            baseQuery = baseQuery.Where(x => x.Zone != null && x.Zone.Id == zoneId.Value);

        if (stationId.HasValue)
            baseQuery = baseQuery.Where(x => x.Station.Id == stationId.Value);

        if (!string.IsNullOrWhiteSpace(request.q))
        {
            var q = request.q.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                (x.Zone != null ? (x.Zone.Name ?? string.Empty).ToLower().Contains(q) : false)
                || (x.Station.Name ?? string.Empty).ToLower().Contains(q)
            );
        }

        var projected = baseQuery.Select(x => new LessPatrolDailyHeaderDto
        {
            DutyDate = x.Patrol.DutyDate,
            LessOperationalZoneId = x.Zone != null ? x.Zone.Id : null,
            LessOperationalZoneName = x.Zone != null ? x.Zone.Name : null,
            StationId = x.Station.Id,
            StationName = x.Station.Name,
            ManDaysPlanned = x.Patrol.ManDaysPlanned,
            ManDaysPerformed = x.Patrol.ManDaysPerformed,
            FootPatrolPlanned = x.Patrol.FootPatrolPlanned,
            FootPatrolPerformed = x.Patrol.FootPatrolPerformed,
            VehiclePatrolPlanned = x.Patrol.VehiclePatrolPlanned,
            VehiclePatrolPerformed = x.Patrol.VehiclePatrolPerformed,
            BoatPatrolPlanned = x.Patrol.BoatPatrolPlanned,
            BoatPatrolPerformed = x.Patrol.BoatPatrolPerformed,
            AirPatrolPlanned = x.Patrol.AirPatrolPlanned,
            AirPatrolPerformed = x.Patrol.AirPatrolPerformed,
            AirPatrolHoursPlanned = x.Patrol.AirPatrolHoursPlanned,
            AirPatrolHoursPerformed = x.Patrol.AirPatrolHoursPerformed,
            AreaInspectedKm = x.Patrol.AreaInspectedKm,
            CreatedAt = x.Patrol.CreatedAt,
            Status = "Recorded",
        });

        var sortBy = (request.sortBy ?? string.Empty).Trim().ToLowerInvariant();
        projected = (sortBy, request.sortDesc) switch
        {
            ("dutydate", false) => projected.OrderBy(x => x.DutyDate),
            ("dutydate", true) => projected.OrderByDescending(x => x.DutyDate),
            ("createdat", false) => projected.OrderBy(x => x.CreatedAt),
            ("createdat", true) => projected.OrderByDescending(x => x.CreatedAt),
            _ => projected.OrderByDescending(x => x.DutyDate),
        };

        return await PagedList<LessPatrolDailyHeaderDto>.CreateAsync(projected, request.page, request.pageSize);
    }

    private void ValidateNonNegative(LessPatrolDailySaveRequest request)
    {
        if (request.ManDaysPlanned < 0 || request.ManDaysPerformed < 0)
            throw new InvalidOperationException("Man-days values cannot be negative");

        if (request.FootPatrolPlanned < 0 || request.FootPatrolPerformed < 0)
            throw new InvalidOperationException("Foot patrol values cannot be negative");

        if (request.VehiclePatrolPlanned < 0 || request.VehiclePatrolPerformed < 0)
            throw new InvalidOperationException("Vehicle patrol values cannot be negative");

        if (request.BoatPatrolPlanned < 0 || request.BoatPatrolPerformed < 0)
            throw new InvalidOperationException("Boat patrol values cannot be negative");

        if (request.AirPatrolPlanned < 0 || request.AirPatrolPerformed < 0)
            throw new InvalidOperationException("Air patrol values cannot be negative");

        if (request.AirPatrolHoursPlanned < 0 || request.AirPatrolHoursPerformed < 0)
            throw new InvalidOperationException("Air patrol hours cannot be negative");

        if (request.AreaInspectedKm < 0)
            throw new InvalidOperationException("Area inspected cannot be negative");
    }

    private DateOnly ParseDateOnlyOrThrow(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new InvalidOperationException("Duty date is required");

        if (!DateOnly.TryParse(s, out var date))
            throw new InvalidOperationException("Invalid duty date format. Expected yyyy-MM-dd");

        return date;
    }

    private async Task<LessScope> GetScopeOrThrow()
    {
        var authorizedParks = _userContext.GetAuthorizedParkIds(_context);
        if (authorizedParks.Count == 1)
            return new LessScope(authorizedParks[0], null);

        var userId = _userContext.GetUserId();

        var staffId = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => u.StaffId)
            .FirstOrDefaultAsync();

        if (staffId == Guid.Empty)
            throw new UnauthorizedAccessException("User is not linked to a staff record");

        var officeIds = await _context.DepartmentStaffs
            .AsNoTracking()
            .Where(ds => ds.StaffId == staffId)
            .Select(ds => ds.OfficeId)
            .Distinct()
            .ToListAsync();

        if (officeIds.Count != 1)
            throw new UnauthorizedAccessException("This action is only allowed for park-scoped or office-scoped users.");

        return new LessScope(null, officeIds[0]);
    }
}
