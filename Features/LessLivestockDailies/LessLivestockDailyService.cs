using conservation_backend.Config;
using conservation_backend.Features.LessLivestockConfig;
using conservation_backend.Features.LessLivestockDailies.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessLivestockDailies;

public class LessLivestockDailyService(
    AppDBContext context,
    IUserContext userContext,
    ILessLivestockDailyRepository repository
) : ILessLivestockDailyService
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    private readonly ILessLivestockDailyRepository _repository = repository;

    private record LessScope(Guid? ParkId, Guid? OfficeId);

    public async Task<LessLivestockDailyResponseDto> GetEntry(LessLivestockDailyGetRequest request)
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

        EnsureStationInScope(scope, station);

        var existing = await _context.LessLivestockDailies
            .AsNoTracking()
            .Include(x => x.Livestock)
            .Include(x => x.Actions)
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate);

        var revenueKeys = await _context.LessLivestockActionOptions
            .AsNoTracking()
            .Where(x => x.IsActive && x.IsRevenue)
            .Select(x => x.Key)
            .ToListAsync();

        var revenueSet = revenueKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var totalRevenue = existing?.Actions
            ?.Where(a => revenueSet.Contains(a.ActionOptionKey))
            .Sum(a => a.Amount) ?? 0m;

        return new LessLivestockDailyResponseDto
        {
            LivestockDailyId = existing?.Id,
            ParkId = scope.ParkId,
            OfficeId = scope.OfficeId,
            LessOperationalZoneId = station.LessOperationalZoneId,
            LessOperationalZoneName = station.LessOperationalZone?.Name,
            StationId = stationId,
            StationName = station.Name,
            DutyDate = dutyDate,
            Livestock = (existing?.Livestock ?? new List<LessLivestockDailyLivestock>())
                .OrderBy(x => x.LivestockTypeKey)
                .Select(x => new LessLivestockDailyLivestockDto
                {
                    LivestockTypeKey = x.LivestockTypeKey,
                    Count = x.Count,
                })
                .ToList(),
            Actions = (existing?.Actions ?? new List<LessLivestockDailyAction>())
                .OrderBy(x => x.LivestockTypeKey)
                .Select(x => new LessLivestockDailyActionDto
                {
                    LivestockTypeKey = x.LivestockTypeKey,
                    ActionOptionKey = x.ActionOptionKey,
                    Description = x.Description,
                    Amount = x.Amount,
                    ControlNumber = x.ControlNumber,
                    CaseNumber = x.CaseNumber,
                })
                .ToList(),
            TotalRevenueAmount = totalRevenue,
        };
    }

    public async Task<LessLivestockDailyResponseDto> SaveEntry(LessLivestockDailySaveRequest request)
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

        EnsureStationInScope(scope, station);

        ValidateNonNegative(request);
        await ValidateActionsAgainstConfig(request.Actions);

        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var existing = await _context.LessLivestockDailies
            .Include(x => x.Livestock)
            .Include(x => x.Actions)
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate);

        if (existing == null)
        {
            existing = new LessLivestockDaily
            {
                Id = Guid.NewGuid(),
                ParkId = scope.ParkId,
                OfficeId = scope.OfficeId,
                LessRangerStationId = stationId,
                DutyDate = dutyDate,
                CreatedAt = now,
                CreatedBy = userId,
            };

            _context.LessLivestockDailies.Add(existing);
        }
        else
        {
            existing.UpdatedAt = now;
            existing.UpdatedBy = userId;
        }

        UpsertLivestock(existing, request.Livestock, now, userId);
        UpsertActions(existing, request.Actions, now, userId);

        await _repository.SaveChanges();

        return await GetEntry(new LessLivestockDailyGetRequest
        {
            StationId = stationId.ToString(),
            DutyDate = dutyDate.ToString("yyyy-MM-dd"),
        });
    }

    public async Task<PagedList<LessLivestockDailyHeaderDto>> GetHeaders(LessLivestockDailyHeadersRequest request)
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

        var revenueKeys = await _context.LessLivestockActionOptions
            .AsNoTracking()
            .Where(x => x.IsActive && x.IsRevenue)
            .Select(x => x.Key)
            .ToListAsync();

        var revenueSet = revenueKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var baseQuery =
            from d in _context.LessLivestockDailies.AsNoTracking()
            join s in _context.LessRangerStations.AsNoTracking() on d.LessRangerStationId equals s.Id
            join z0 in _context.LessOperationalZones.AsNoTracking() on s.LessOperationalZoneId equals z0.Id into zg
            from z in zg.DefaultIfEmpty()
            where (scope.ParkId.HasValue && d.ParkId == scope.ParkId.Value)
                  || (scope.OfficeId.HasValue && d.OfficeId == scope.OfficeId.Value)
            select new
            {
                Daily = d,
                Station = s,
                Zone = z,
            };

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Daily.DutyDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Daily.DutyDate <= toDate.Value);

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

        var projected = baseQuery.Select(x => new LessLivestockDailyHeaderDto
        {
            DutyDate = x.Daily.DutyDate,
            LessOperationalZoneId = x.Zone != null ? x.Zone.Id : null,
            LessOperationalZoneName = x.Zone != null ? x.Zone.Name : null,
            StationId = x.Station.Id,
            StationName = x.Station.Name,
            TotalInCustody = _context.LessLivestockDailyLivestock
                .AsNoTracking()
                .Where(l => l.LessLivestockDailyId == x.Daily.Id)
                .Sum(l => (int?)l.Count) ?? 0,
            TotalRevenueAmount = _context.LessLivestockDailyActions
                .AsNoTracking()
                .Where(a => a.LessLivestockDailyId == x.Daily.Id && revenueSet.Contains(a.ActionOptionKey))
                .Sum(a => (decimal?)a.Amount) ?? 0m,
            CreatedAt = x.Daily.CreatedAt,
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

        return await PagedList<LessLivestockDailyHeaderDto>.CreateAsync(projected, request.page, request.pageSize);
    }

    private void UpsertLivestock(
        LessLivestockDaily daily,
        List<LessLivestockDailyLivestockDto> incoming,
        DateTime now,
        Guid? userId
    )
    {
        daily.Livestock ??= new List<LessLivestockDailyLivestock>();

        var byKey = daily.Livestock
            .ToDictionary(x => x.LivestockTypeKey, StringComparer.OrdinalIgnoreCase);

        var incomingKeys = incoming
            .Select(x => (x.LivestockTypeKey ?? string.Empty).Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var existing in daily.Livestock)
        {
            if (!incomingKeys.Contains(existing.LivestockTypeKey))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var item in incoming)
        {
            var key = (item.LivestockTypeKey ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key))
                continue;

            if (byKey.TryGetValue(key, out var existing))
            {
                existing.Count = item.Count;
                existing.DeletedAt = null;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
            else
            {
                daily.Livestock.Add(new LessLivestockDailyLivestock
                {
                    LessLivestockDailyId = daily.Id,
                    LivestockTypeKey = key,
                    Count = item.Count,
                    CreatedAt = now,
                    CreatedBy = userId,
                });
            }
        }
    }

    private void UpsertActions(
        LessLivestockDaily daily,
        List<LessLivestockDailyActionDto> incoming,
        DateTime now,
        Guid? userId
    )
    {
        daily.Actions ??= new List<LessLivestockDailyAction>();

        var byType = daily.Actions
            .ToDictionary(x => x.LivestockTypeKey, StringComparer.OrdinalIgnoreCase);

        var incomingKeys = incoming
            .Select(x => (x.LivestockTypeKey ?? string.Empty).Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var existing in daily.Actions)
        {
            if (!incomingKeys.Contains(existing.LivestockTypeKey))
            {
                existing.DeletedAt = now;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
        }

        foreach (var item in incoming)
        {
            var typeKey = (item.LivestockTypeKey ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(typeKey))
                continue;

            var actionKey = (item.ActionOptionKey ?? string.Empty).Trim();

            if (byType.TryGetValue(typeKey, out var existing))
            {
                existing.ActionOptionKey = actionKey;
                existing.Description = (item.Description ?? string.Empty).Trim();
                existing.Amount = item.Amount;
                existing.ControlNumber = (item.ControlNumber ?? string.Empty).Trim();
                existing.CaseNumber = (item.CaseNumber ?? string.Empty).Trim();
                existing.DeletedAt = null;
                existing.UpdatedAt = now;
                existing.UpdatedBy = userId;
            }
            else
            {
                daily.Actions.Add(new LessLivestockDailyAction
                {
                    LessLivestockDailyId = daily.Id,
                    LivestockTypeKey = typeKey,
                    ActionOptionKey = actionKey,
                    Description = (item.Description ?? string.Empty).Trim(),
                    Amount = item.Amount,
                    ControlNumber = (item.ControlNumber ?? string.Empty).Trim(),
                    CaseNumber = (item.CaseNumber ?? string.Empty).Trim(),
                    CreatedAt = now,
                    CreatedBy = userId,
                });
            }
        }
    }

    private void EnsureStationInScope(LessScope scope, LessRangerStations.LessRangerStation station)
    {
        if (scope.ParkId.HasValue)
        {
            if (station.LessOperationalZone is null)
                throw new UnauthorizedAccessException("You are not authorized to access this station");

            if (station.LessOperationalZone.ParkId != scope.ParkId.Value)
                throw new UnauthorizedAccessException("You are not authorized to access this station");

            return;
        }

        if (!scope.OfficeId.HasValue)
            throw new UnauthorizedAccessException("Office is required for office-scoped users");

        if (station.OfficeId != scope.OfficeId.Value)
            throw new UnauthorizedAccessException("You are not authorized to access this station");
    }

    private void ValidateNonNegative(LessLivestockDailySaveRequest request)
    {
        foreach (var item in request.Livestock)
        {
            if (item.Count < 0)
                throw new InvalidOperationException("Livestock count cannot be negative");
        }

        foreach (var action in request.Actions)
        {
            if (action.Amount < 0)
                throw new InvalidOperationException("Amount cannot be negative");
        }
    }

    private async Task ValidateActionsAgainstConfig(List<LessLivestockDailyActionDto> actions)
    {
        if (actions.Count == 0) return;

        var keys = actions
            .Select(a => (a.ActionOptionKey ?? string.Empty).Trim())
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (keys.Count == 0) return;

        var cfg = await _context.LessLivestockActionOptions
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Where(x => keys.Contains(x.Key))
            .Select(x => new { x.Key, x.RequiresControlNumber, x.RequiresCaseNumber })
            .ToListAsync();

        var map = cfg.ToDictionary(x => x.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var action in actions)
        {
            var key = (action.ActionOptionKey ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key)) continue;

            if (!map.TryGetValue(key, out var rule))
                throw new InvalidOperationException($"Invalid action option: {key}");

            if (rule.RequiresControlNumber && string.IsNullOrWhiteSpace(action.ControlNumber))
                throw new InvalidOperationException($"Control number is required for action option: {key}");

            if (rule.RequiresCaseNumber && string.IsNullOrWhiteSpace(action.CaseNumber))
                throw new InvalidOperationException($"Case number is required for action option: {key}");
        }
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
