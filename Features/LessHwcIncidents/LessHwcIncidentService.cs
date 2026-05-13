using System.Text.Json;
using conservation_backend.Config;
using conservation_backend.Features.LessHwcConfig;
using conservation_backend.Features.LessHwcIncidents.Interfaces;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessHwcIncidents;

public class LessHwcIncidentService(
    AppDBContext context,
    IUserContext userContext,
    ILessHwcIncidentRepository repository
) : ILessHwcIncidentService
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    private readonly ILessHwcIncidentRepository _repository = repository;

    private enum LessHwcPhase
    {
        Report,
        Response,
        Followup,
    }

    private record LessScope(Guid? ParkId, Guid? OfficeId, List<Guid> AuthorizedParks);

    public async Task<LessHwcIncidentResponseDto> GetById(Guid id)
    {
        var scope = await GetScopeOrThrow();

        var incident = await _context.LessHwcIncidents
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (incident is null)
            throw new KeyNotFoundException("HWC incident not found");

        EnsureIncidentInScope(scope, incident);

        return ToResponseDto(incident);
    }

    public async Task<LessHwcIncidentResponseDto> Create(LessHwcIncidentCreateRequest request)
    {
        var scope = await GetScopeOrThrow();

        var parkId = ResolveParkIdOrThrow(scope, request.ParkId);
        var date = ParseDateOnlyOrThrow(request.IncidentDate);

        var phase = ParsePhaseOrDefault(request.Phase, LessHwcPhase.Report);
        if (phase != LessHwcPhase.Report)
            throw new InvalidOperationException("You must first record the incident before adding response/follow-up");

        var data = NormalizeData(request.Data);
        data["tarehe"] = date.ToString("yyyy-MM-dd");

        var allowedTabs = GetAllowedTabKeys(phase);

        var allowedKeys = await _context.LessHwcFieldDefinitions
            .AsNoTracking()
            .Include(x => x.TabDefinition)
            .Where(x => x.IsActive)
            .Where(x => x.TabDefinition.IsActive)
            .Where(x => allowedTabs.Contains((x.TabDefinition.Key ?? string.Empty).Trim()))
            .Select(x => (x.Key ?? string.Empty).Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToListAsync();

        var allowedKeySet = allowedKeys
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        allowedKeySet.Add("tarehe");

        data = data
            .Where(kv => allowedKeySet.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);

        await ApplyComputedFields(data, allowedTabs);
        await ValidateAgainstConfig(data, allowedTabs);

        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var incident = new LessHwcIncident
        {
            Id = Guid.NewGuid(),
            ParkId = parkId,
            OfficeId = scope.OfficeId,
            IncidentDate = date,
            Status = StatusForPhase(phase),
            District = (data.GetValueOrDefault("wilaya") ?? string.Empty).Trim(),
            Ward = (data.GetValueOrDefault("kata") ?? string.Empty).Trim(),
            Villages = (data.GetValueOrDefault("kijiji") ?? string.Empty).Trim(),
            IncidentCategoryKey = (data.GetValueOrDefault("ainaYaUharibifu") ?? string.Empty).Trim(),
            ReferenceNo = (data.GetValueOrDefault("kumbukumbuNa") ?? string.Empty).Trim(),
            TotalIncidents = ToInt(data.GetValueOrDefault("jumla")),
            EstimatedLossTzs = ToDecimal(data.GetValueOrDefault("makadirioYaHasara")),
            DataJson = JsonSerializer.Serialize(data),
            CreatedAt = now,
            CreatedBy = userId,
        };

        _context.LessHwcIncidents.Add(incident);
        await _repository.SaveChanges();

        return await GetById(incident.Id);
    }

    public async Task<LessHwcIncidentResponseDto> Update(Guid id, LessHwcIncidentUpdateRequest request)
    {
        var scope = await GetScopeOrThrow();

        var incident = await _context.LessHwcIncidents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (incident is null)
            throw new KeyNotFoundException("HWC incident not found");

        EnsureIncidentInScope(scope, incident);

        var parkId = ResolveParkIdOrThrow(scope, request.ParkId ?? incident.ParkId.ToString());
        var date = ParseDateOnlyOrThrow(request.IncidentDate);

        var requestedPhase = ParsePhaseOrDefault(request.Phase, PhaseForStatus(incident.Status));
        EnsureValidTransition(incident.Status, requestedPhase);

        var incoming = NormalizeData(request.Data);
        incoming["tarehe"] = date.ToString("yyyy-MM-dd");

        var allowedTabs = GetAllowedTabKeys(requestedPhase);

        var allowedKeys = await _context.LessHwcFieldDefinitions
            .AsNoTracking()
            .Include(x => x.TabDefinition)
            .Where(x => x.IsActive)
            .Where(x => x.TabDefinition.IsActive)
            .Where(x => allowedTabs.Contains((x.TabDefinition.Key ?? string.Empty).Trim()))
            .Select(x => (x.Key ?? string.Empty).Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToListAsync();

        var allowedKeySet = allowedKeys
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        allowedKeySet.Add("tarehe");

        var data = DeserializeDataJson(incident.DataJson);
        foreach (var (k, v) in incoming)
        {
            if (!allowedKeySet.Contains(k))
                continue;
            data[k] = v;
        }

        await ApplyComputedFields(data, allowedTabs);
        await ValidateAgainstConfig(data, allowedTabs);

        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        incident.ParkId = parkId;
        incident.OfficeId = scope.OfficeId;
        incident.IncidentDate = date;
        incident.Status = MergeStatus(incident.Status, StatusForPhase(requestedPhase));
        incident.District = (data.GetValueOrDefault("wilaya") ?? string.Empty).Trim();
        incident.Ward = (data.GetValueOrDefault("kata") ?? string.Empty).Trim();
        incident.Villages = (data.GetValueOrDefault("kijiji") ?? string.Empty).Trim();
        incident.IncidentCategoryKey = (data.GetValueOrDefault("ainaYaUharibifu") ?? string.Empty).Trim();
        incident.ReferenceNo = (data.GetValueOrDefault("kumbukumbuNa") ?? string.Empty).Trim();
        incident.TotalIncidents = ToInt(data.GetValueOrDefault("jumla"));
        incident.EstimatedLossTzs = ToDecimal(data.GetValueOrDefault("makadirioYaHasara"));
        incident.DataJson = JsonSerializer.Serialize(data);
        incident.UpdatedAt = now;
        incident.UpdatedBy = userId;
        incident.DeletedAt = null;

        await _repository.SaveChanges();

        return await GetById(id);
    }

    public async Task<bool> Delete(Guid id)
    {
        var scope = await GetScopeOrThrow();

        var incident = await _context.LessHwcIncidents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (incident is null)
            throw new KeyNotFoundException("HWC incident not found");

        EnsureIncidentInScope(scope, incident);

        var now = DateTime.UtcNow;
        incident.DeletedAt = now;
        incident.UpdatedAt = now;
        incident.UpdatedBy = _userContext.GetUserId();

        await _repository.SaveChanges();
        return true;
    }

    public async Task<PagedList<LessHwcIncidentHeaderDto>> GetHeaders(LessHwcIncidentHeadersRequest request)
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

        Guid? parkIdFilter = null;
        if (!string.IsNullOrWhiteSpace(request.ParkId))
        {
            if (!Guid.TryParse(request.ParkId, out var parsed))
                throw new InvalidOperationException("Invalid park id");
            parkIdFilter = parsed;
        }

        var baseQuery =
            from i in _context.LessHwcIncidents.AsNoTracking()
            join p in _context.Parks.AsNoTracking() on i.ParkId equals p.Id
            select new { Incident = i, Park = p };

        if (scope.ParkId.HasValue)
            baseQuery = baseQuery.Where(x => x.Incident.ParkId == scope.ParkId.Value);
        else if (scope.OfficeId.HasValue)
            baseQuery = baseQuery.Where(x => x.Incident.OfficeId == scope.OfficeId.Value);

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Incident.IncidentDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Incident.IncidentDate <= toDate.Value);

        if (parkIdFilter.HasValue)
            baseQuery = baseQuery.Where(x => x.Incident.ParkId == parkIdFilter.Value);

        if (!string.IsNullOrWhiteSpace(request.IncidentCategoryKey))
        {
            var key = request.IncidentCategoryKey.Trim();
            baseQuery = baseQuery.Where(x => x.Incident.IncidentCategoryKey == key);
        }

        if (!string.IsNullOrWhiteSpace(request.q))
        {
            var q = request.q.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                (x.Incident.District ?? string.Empty).ToLower().Contains(q)
                || (x.Incident.Ward ?? string.Empty).ToLower().Contains(q)
                || (x.Incident.Villages ?? string.Empty).ToLower().Contains(q)
                || (x.Incident.ReferenceNo ?? string.Empty).ToLower().Contains(q)
            );
        }

        var projected = baseQuery.Select(x => new LessHwcIncidentHeaderDto
        {
            Id = x.Incident.Id,
            ParkId = x.Incident.ParkId,
            ParkName = x.Park.Name,
            OfficeId = x.Incident.OfficeId,
            IncidentDate = x.Incident.IncidentDate,
            District = x.Incident.District,
            Ward = x.Incident.Ward,
            Villages = x.Incident.Villages,
            IncidentCategoryKey = x.Incident.IncidentCategoryKey,
            TotalIncidents = x.Incident.TotalIncidents,
            EstimatedLossTzs = x.Incident.EstimatedLossTzs,
            ReferenceNo = x.Incident.ReferenceNo,
            CreatedAt = x.Incident.CreatedAt,
            Status = x.Incident.Status,
        });

        var sortBy = (request.sortBy ?? string.Empty).Trim().ToLowerInvariant();
        projected = (sortBy, request.sortDesc) switch
        {
            ("incidentdate", false) => projected.OrderBy(x => x.IncidentDate),
            ("incidentdate", true) => projected.OrderByDescending(x => x.IncidentDate),
            ("createdat", false) => projected.OrderBy(x => x.CreatedAt),
            ("createdat", true) => projected.OrderByDescending(x => x.CreatedAt),
            _ => projected.OrderByDescending(x => x.IncidentDate),
        };

        return await PagedList<LessHwcIncidentHeaderDto>.CreateAsync(projected, request.page, request.pageSize);
    }

    private LessHwcIncidentResponseDto ToResponseDto(LessHwcIncident incident)
    {
        var data = DeserializeDataJson(incident.DataJson);

        return new LessHwcIncidentResponseDto
        {
            Id = incident.Id,
            ParkId = incident.ParkId,
            OfficeId = incident.OfficeId,
            IncidentDate = incident.IncidentDate,
            Status = incident.Status,
            Data = data,
            CreatedAt = incident.CreatedAt,
            UpdatedAt = incident.UpdatedAt,
        };
    }

    private Dictionary<string, string?> DeserializeDataJson(string? json)
    {
        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string?>>(json ?? "{}")
                   ?? new Dictionary<string, string?>();
        }
        catch
        {
            return new Dictionary<string, string?>();
        }
    }

    private Dictionary<string, string?> NormalizeData(Dictionary<string, string?>? incoming)
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        if (incoming == null) return data;

        foreach (var (k, v) in incoming)
        {
            var key = (k ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key)) continue;
            data[key] = v;
        }

        return data;
    }

    private async Task ApplyComputedFields(Dictionary<string, string?> data, HashSet<string> allowedTabKeys)
    {
        var computedFields = await _context.LessHwcFieldDefinitions
            .AsNoTracking()
            .Include(x => x.TabDefinition)
            .Where(x => x.IsActive)
            .Where(x => x.TabDefinition.IsActive)
            .Where(x => x.IsComputed)
            .ToListAsync();

        foreach (var field in computedFields)
        {
            if (!allowedTabKeys.Contains((field.TabDefinition.Key ?? string.Empty).Trim()))
                continue;

            var key = (field.Key ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key))
                continue;

            var existingValue = data.GetValueOrDefault(key);
            if (!string.IsNullOrWhiteSpace(existingValue))
                continue;

            var fromKeys = (field.ComputeFromKeys ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            if (fromKeys.Count == 0)
                continue;

            var sum = fromKeys
                .Select(k => ToDecimal(data.GetValueOrDefault(k)))
                .Sum();

            if (string.Equals(field.FieldType, "number", StringComparison.OrdinalIgnoreCase))
                data[key] = sum.ToString("0.##");
            else
                data[key] = sum.ToString("0.##");
        }
    }

    private LessHwcPhase ParsePhaseOrDefault(string? phase, LessHwcPhase fallback)
    {
        var p = (phase ?? string.Empty).Trim().ToLowerInvariant();
        return p switch
        {
            "report" => LessHwcPhase.Report,
            "response" => LessHwcPhase.Response,
            "followup" => LessHwcPhase.Followup,
            "follow-up" => LessHwcPhase.Followup,
            "follow_up" => LessHwcPhase.Followup,
            _ => fallback,
        };
    }

    private HashSet<string> GetAllowedTabKeys(LessHwcPhase phase)
    {
        // user-selected mapping:
        // report: location + incidents
        // response: + response
        // followup: + followup
        var keys = phase switch
        {
            LessHwcPhase.Report => new[] { "location", "incidents" },
            LessHwcPhase.Response => new[] { "location", "incidents", "response" },
            LessHwcPhase.Followup => new[] { "location", "incidents", "response", "followup" },
            _ => new[] { "location", "incidents" },
        };

        return keys
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private string StatusForPhase(LessHwcPhase phase)
    {
        return phase switch
        {
            LessHwcPhase.Report => "Reported",
            LessHwcPhase.Response => "Responded",
            LessHwcPhase.Followup => "FollowedUp",
            _ => "Reported",
        };
    }

    private LessHwcPhase PhaseForStatus(string? status)
    {
        var s = (status ?? string.Empty).Trim().ToLowerInvariant();
        return s switch
        {
            "responded" => LessHwcPhase.Response,
            "followedup" => LessHwcPhase.Followup,
            "followed_up" => LessHwcPhase.Followup,
            "followed-up" => LessHwcPhase.Followup,
            _ => LessHwcPhase.Report,
        };
    }

    private int StatusOrder(string? status)
    {
        var s = (status ?? string.Empty).Trim().ToLowerInvariant();
        return s switch
        {
            "followedup" => 3,
            "followed_up" => 3,
            "followed-up" => 3,
            "responded" => 2,
            "reported" => 1,
            _ => 1,
        };
    }

    private string MergeStatus(string currentStatus, string requestedStatus)
    {
        return StatusOrder(requestedStatus) >= StatusOrder(currentStatus)
            ? requestedStatus
            : currentStatus;
    }

    private void EnsureValidTransition(string currentStatus, LessHwcPhase requestedPhase)
    {
        var currentPhase = PhaseForStatus(currentStatus);
        if (requestedPhase == LessHwcPhase.Followup && currentPhase != LessHwcPhase.Response && currentPhase != LessHwcPhase.Followup)
            throw new InvalidOperationException("You must complete response phase before follow-up");
    }

    private async Task ValidateAgainstConfig(Dictionary<string, string?> data, HashSet<string> allowedTabKeys)
    {
        var fields = await _context.LessHwcFieldDefinitions
            .AsNoTracking()
            .Include(x => x.TabDefinition)
            .Include(x => x.Options)
            .Where(x => x.IsActive)
            .Where(x => x.TabDefinition.IsActive)
            .ToListAsync();

        foreach (var field in fields)
        {
            if (!allowedTabKeys.Contains((field.TabDefinition.Key ?? string.Empty).Trim()))
                continue;

            var key = (field.Key ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(key))
                continue;

            var value = (data.GetValueOrDefault(key) ?? string.Empty).Trim();

            if (!field.IsComputed && field.IsRequired && string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"{field.Label} is required");

            var type = (field.FieldType ?? "text").Trim().ToLowerInvariant();

            if (type == "number" && !string.IsNullOrWhiteSpace(value))
            {
                if (!decimal.TryParse(value, out var d))
                    throw new InvalidOperationException($"{field.Label} must be a number");
            }

            if ((type == "select" || type == "multiselect") && !string.IsNullOrWhiteSpace(value))
            {
                var allowed = field.Options
                    .Where(o => o.IsActive)
                    .Select(o => (o.Value ?? string.Empty).Trim())
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (allowed.Count > 0)
                {
                    if (type == "select")
                    {
                        if (!allowed.Contains(value))
                            throw new InvalidOperationException($"Invalid option for {field.Label}");
                    }
                    else
                    {
                        var selected = value
                            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                            .ToList();

                        foreach (var s in selected)
                        {
                            if (!allowed.Contains(s))
                                throw new InvalidOperationException($"Invalid option for {field.Label}");
                        }
                    }
                }
            }
        }
    }

    private void EnsureIncidentInScope(LessScope scope, LessHwcIncident incident)
    {
        if (scope.ParkId.HasValue)
        {
            if (incident.ParkId != scope.ParkId.Value)
                throw new UnauthorizedAccessException("You are not authorized to access this record");
            return;
        }

        if (!scope.OfficeId.HasValue)
            throw new UnauthorizedAccessException("Office is required for office-scoped users");

        if (incident.OfficeId != scope.OfficeId.Value)
            throw new UnauthorizedAccessException("You are not authorized to access this record");

        if (!scope.AuthorizedParks.Contains(incident.ParkId))
            throw new UnauthorizedAccessException("You are not authorized to access this park");
    }

    private Guid ResolveParkIdOrThrow(LessScope scope, string? requestedParkId)
    {
        if (scope.ParkId.HasValue)
            return scope.ParkId.Value;

        if (string.IsNullOrWhiteSpace(requestedParkId) || !Guid.TryParse(requestedParkId, out var parsed))
            throw new InvalidOperationException("Park is required");

        if (!scope.AuthorizedParks.Contains(parsed))
            throw new UnauthorizedAccessException("You are not authorized to access this park");

        return parsed;
    }

    private DateOnly ParseDateOnlyOrThrow(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new InvalidOperationException("Incident date is required");

        if (!DateOnly.TryParse(s, out var date))
            throw new InvalidOperationException("Invalid incident date format. Expected yyyy-MM-dd");

        return date;
    }

    private async Task<LessScope> GetScopeOrThrow()
    {
        var authorizedParks = _userContext.GetAuthorizedParkIds(_context);
        if (authorizedParks.Count == 1)
            return new LessScope(authorizedParks[0], null, authorizedParks);

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

        return new LessScope(null, officeIds[0], authorizedParks);
    }

    private int ToInt(string? s)
    {
        if (int.TryParse((s ?? string.Empty).Trim(), out var i))
            return i;
        return 0;
    }

    private decimal ToDecimal(string? s)
    {
        if (decimal.TryParse((s ?? string.Empty).Trim(), out var d))
            return d;
        return 0m;
    }
}
