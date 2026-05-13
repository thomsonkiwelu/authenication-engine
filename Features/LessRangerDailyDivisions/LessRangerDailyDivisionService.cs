using conservation_backend.Config;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessRangerDailyDivisions.Interfaces;
using conservation_backend.Features.LessRangerDivisionConfig;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.LessStaffPostings;
using conservation_backend.Shared;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessRangerDailyDivisions;

public class LessRangerDailyDivisionService(
    AppDBContext context,
    IUserContext userContext,
    ILessRangerDailyDivisionRepository repository
) : ILessRangerDailyDivisionService
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    private readonly ILessRangerDailyDivisionRepository _repository = repository;

    private record LessScope(Guid? ParkId, Guid? OfficeId);

    public async Task<LessRangerDailyDivisionResponseDto> GetDivision(LessRangerDailyDivisionGetRequest request)
    {
        var scope = await GetScopeOrThrow();
        var category = NormalizeCategoryOrThrow(request.Category);

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

        var fields = await _context.LessRangerDutyFieldDefinitions
            .AsNoTracking()
            .Where(x => x.Category == category && x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Select(x => new { x.Id, x.Key, x.Label, x.SortOrder })
            .ToListAsync();

        var roster = await GetStationRoster(scope, stationId, dutyDate, category);

        var division = await _repository.GetByStationDateCategory(stationId, dutyDate, category);

        var assignmentsByField = new Dictionary<Guid, List<StaffOptionDto>>();

        if (division != null)
        {
            var assignments = await _repository.GetAssignments(division.Id);

            foreach (var a in assignments)
            {
                if (!assignmentsByField.TryGetValue(a.DutyFieldDefinitionId, out var list))
                {
                    list = new List<StaffOptionDto>();
                    assignmentsByField[a.DutyFieldDefinitionId] = list;
                }

                list.Add(new StaffOptionDto
                {
                    Id = a.StaffId,
                    Name = a.Staff.FirstName + " " + a.Staff.LastName,
                    RankId = a.Staff.RankId,
                    RankName = a.Staff.Rank.Name,
                });
            }

            foreach (var kv in assignmentsByField)
                kv.Value.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
        }

        var fieldDtos = fields
            .Select(f => new LessRangerDailyDivisionFieldAssignmentDto
            {
                DutyFieldDefinitionId = f.Id,
                Key = f.Key,
                Label = f.Label,
                SortOrder = f.SortOrder,
                Staff = assignmentsByField.TryGetValue(f.Id, out var list) ? list : new List<StaffOptionDto>(),
            })
            .ToList();

        return new LessRangerDailyDivisionResponseDto
        {
            DivisionId = division?.Id,
            ParkId = scope.ParkId,
            OfficeId = scope.OfficeId,
            StationId = stationId,
            StationName = station.Name,
            DutyDate = dutyDate,
            Category = category,
            StationRoster = roster,
            Fields = fieldDtos,
        };
    }

    public async Task<LessRangerDailyDivisionResponseDto> SaveDivision(LessRangerDailyDivisionSaveRequest request)
    {
        var scope = await GetScopeOrThrow();
        var category = NormalizeCategoryOrThrow(request.Category);

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

        var activeFieldIds = await _context.LessRangerDutyFieldDefinitions
            .AsNoTracking()
            .Where(x => x.Category == category && x.IsActive)
            .Select(x => x.Id)
            .ToListAsync();

        var activeFieldSet = activeFieldIds.ToHashSet();

        var incoming = new Dictionary<Guid, List<Guid>>();

        foreach (var f in request.Fields ?? new List<LessRangerDailyDivisionSaveFieldRequest>())
        {
            if (!Guid.TryParse(f.DutyFieldDefinitionId, out var fieldId))
                continue;

            if (!activeFieldSet.Contains(fieldId))
                throw new InvalidOperationException("One or more selected duty fields are invalid for the chosen category");

            var staffIds = (f.StaffIds ?? new List<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(Guid.Parse)
                .Distinct()
                .ToList();

            incoming[fieldId] = staffIds;
        }

        var allStaff = incoming.SelectMany(x => x.Value).ToList();
        if (allStaff.Count != allStaff.Distinct().Count())
            throw new InvalidOperationException("A staff member can only be assigned to one duty field per date");

        var roster = await GetStationRoster(scope, stationId, dutyDate, category);
        var rosterSet = roster.Select(x => x.Id).ToHashSet();

        foreach (var sid in allStaff)
        {
            if (!rosterSet.Contains(sid))
                throw new InvalidOperationException("One or more selected staff are not available in the station roster");
        }

        var now = DateTime.UtcNow;
        var userId = _userContext.GetUserId();

        var existingDivision = await _context.LessRangerDailyDivisions
            .FirstOrDefaultAsync(x => x.LessRangerStationId == stationId && x.DutyDate == dutyDate && x.Category == category);

        if (existingDivision == null)
        {
            existingDivision = new LessRangerDailyDivision
            {
                Id = Guid.NewGuid(),
                ParkId = scope.ParkId,
                OfficeId = scope.OfficeId,
                LessRangerStationId = stationId,
                DutyDate = dutyDate,
                Category = category,
                CreatedAt = now,
                CreatedBy = userId,
            };

            _context.LessRangerDailyDivisions.Add(existingDivision);
            await _context.SaveChangesAsync();
        }

        var existingAssignments = await _repository.GetAssignmentsIncludingDeleted(existingDivision.Id);
        var existingMap = existingAssignments.ToDictionary(x => x.StaffId, x => x);

        var desiredStaffToField = incoming
            .SelectMany(kv => kv.Value.Select(staffId => new { staffId, fieldId = kv.Key }))
            .ToDictionary(x => x.staffId, x => x.fieldId);

        foreach (var existing in existingAssignments)
        {
            if (!desiredStaffToField.TryGetValue(existing.StaffId, out var desiredFieldId))
            {
                if (existing.DeletedAt == null)
                {
                    existing.DeletedAt = now;
                    existing.UpdatedAt = now;
                    existing.UpdatedBy = userId;
                }

                continue;
            }

            existing.DutyFieldDefinitionId = desiredFieldId;
            existing.DeletedAt = null;
            existing.UpdatedAt = now;
            existing.UpdatedBy = userId;
        }

        foreach (var kv in desiredStaffToField)
        {
            if (existingMap.ContainsKey(kv.Key))
                continue;

            _context.LessRangerDailyDivisionAssignments.Add(new LessRangerDailyDivisionAssignment
            {
                Id = Guid.NewGuid(),
                LessRangerDailyDivisionId = existingDivision.Id,
                DutyFieldDefinitionId = kv.Value,
                StaffId = kv.Key,
                CreatedAt = now,
                CreatedBy = userId,
            });
        }

        await _repository.SaveChanges();

        return await GetDivision(new LessRangerDailyDivisionGetRequest
        {
            StationId = stationId.ToString(),
            DutyDate = dutyDate.ToString("yyyy-MM-dd"),
            Category = category,
        });
    }

    public async Task<PagedList<LessRangerDailyDivisionHeaderDto>> GetHeaders(LessRangerDailyDivisionHeadersRequest request)
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

        var categoryFilter = string.IsNullOrWhiteSpace(request.Category)
            ? null
            : NormalizeCategoryOrThrow(request.Category);

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

        var activeAssignments = _context.LessRangerDailyDivisionAssignments
            .AsNoTracking()
            .Where(a => a.DeletedAt == null);

        var baseQuery =
            from d in _context.LessRangerDailyDivisions.AsNoTracking()
            join s in _context.LessRangerStations.AsNoTracking() on d.LessRangerStationId equals s.Id
            join z0 in _context.LessOperationalZones.AsNoTracking() on s.LessOperationalZoneId equals z0.Id into zg
            from z in zg.DefaultIfEmpty()
            join a in activeAssignments on d.Id equals a.LessRangerDailyDivisionId into da
            where (scope.ParkId.HasValue && d.ParkId == scope.ParkId.Value)
                  || (scope.OfficeId.HasValue && d.OfficeId == scope.OfficeId.Value)
            select new
            {
                Division = d,
                Station = s,
                ZoneId = z != null ? z.Id : Guid.Empty,
                ZoneName = z != null ? z.Name : string.Empty,
                AssignmentCount = da.Count(),
            };

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Division.DutyDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.Division.DutyDate <= toDate.Value);

        if (scope.ParkId.HasValue && zoneId.HasValue)
            baseQuery = baseQuery.Where(x => x.ZoneId == zoneId.Value);

        if (stationId.HasValue)
            baseQuery = baseQuery.Where(x => x.Station.Id == stationId.Value);

        if (!string.IsNullOrWhiteSpace(request.q))
        {
            var q = request.q.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                (x.Station.Name ?? string.Empty).ToLower().Contains(q)
                || (x.ZoneName ?? string.Empty).ToLower().Contains(q)
            );
        }

        var grouped =
            from x in baseQuery
            group x by new { x.Division.DutyDate, StationId = x.Station.Id, x.ZoneId } into g
            select new
            {
                g.Key.DutyDate,
                g.Key.StationId,
                ZoneId = g.Key.ZoneId,
                StationName = g.Select(a => a.Station.Name).FirstOrDefault() ?? string.Empty,
                ZoneName = g.Select(a => a.ZoneName).FirstOrDefault() ?? string.Empty,
                HasAskariDivision = g.Any(a => a.Division.Category == "ranger"),
                HasOfficerDivision = g.Any(a => a.Division.Category == "officer"),
                TotalAskari = g.Where(a => a.Division.Category == "ranger").Sum(a => a.AssignmentCount),
                TotalOfficers = g.Where(a => a.Division.Category == "officer").Sum(a => a.AssignmentCount),
                CreatedAt = g.Max(a => a.Division.CreatedAt),
            };

        if (!string.IsNullOrWhiteSpace(categoryFilter))
        {
            grouped = grouped.Where(x => categoryFilter == "ranger" ? x.HasAskariDivision : x.HasOfficerDivision);
        }

        var headerQuery = grouped.Select(x => new LessRangerDailyDivisionHeaderDto
        {
            DutyDate = x.DutyDate,
            LessOperationalZoneId = x.ZoneId == Guid.Empty ? null : x.ZoneId,
            LessOperationalZoneName = x.ZoneName,
            StationId = x.StationId,
            StationName = x.StationName,
            TotalAskari = x.TotalAskari,
            TotalOfficers = x.TotalOfficers,
            HasAskariDivision = x.HasAskariDivision,
            HasOfficerDivision = x.HasOfficerDivision,
            CreatedAt = x.CreatedAt,
            Status = x.HasAskariDivision && x.HasOfficerDivision
                ? "Complete"
                : (x.HasAskariDivision || x.HasOfficerDivision ? "Partial" : "New"),
        });

        var sortBy = (request.sortBy ?? string.Empty).Trim().ToLowerInvariant();
        headerQuery = (sortBy, request.sortDesc) switch
        {
            ("dutydate", true) => headerQuery.OrderByDescending(x => x.DutyDate).ThenBy(x => x.StationName),
            ("dutydate", false) => headerQuery.OrderBy(x => x.DutyDate).ThenBy(x => x.StationName),

            ("stationname", true) => headerQuery.OrderByDescending(x => x.StationName).ThenByDescending(x => x.DutyDate),
            ("stationname", false) => headerQuery.OrderBy(x => x.StationName).ThenByDescending(x => x.DutyDate),

            ("zonename", true) => headerQuery.OrderByDescending(x => x.LessOperationalZoneName).ThenByDescending(x => x.DutyDate),
            ("zonename", false) => headerQuery.OrderBy(x => x.LessOperationalZoneName).ThenByDescending(x => x.DutyDate),

            ("totalaskari", true) => headerQuery.OrderByDescending(x => x.TotalAskari).ThenByDescending(x => x.DutyDate),
            ("totalaskari", false) => headerQuery.OrderBy(x => x.TotalAskari).ThenByDescending(x => x.DutyDate),

            ("totalofficers", true) => headerQuery.OrderByDescending(x => x.TotalOfficers).ThenByDescending(x => x.DutyDate),
            ("totalofficers", false) => headerQuery.OrderBy(x => x.TotalOfficers).ThenByDescending(x => x.DutyDate),

            ("createdat", false) => headerQuery.OrderBy(x => x.CreatedAt),
            _ => headerQuery.OrderByDescending(x => x.CreatedAt),
        };

        return await PagedList<LessRangerDailyDivisionHeaderDto>.CreateAsync(headerQuery, request.page, request.pageSize);
    }

    public async Task<PagedList<LessRangerDailyDivisionPerFieldReportRowDto>> GetPerFieldReport(
        LessRangerDailyDivisionPerFieldReportRequest request
    )
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

        var categoryFilter = string.IsNullOrWhiteSpace(request.Category)
            ? null
            : NormalizeCategoryOrThrow(request.Category);

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

        Guid? fieldId = null;
        if (!string.IsNullOrWhiteSpace(request.DutyFieldDefinitionId))
        {
            if (!Guid.TryParse(request.DutyFieldDefinitionId, out var parsed))
                throw new InvalidOperationException("Invalid duty field definition id");
            fieldId = parsed;
        }

        var activeAssignments = _context.LessRangerDailyDivisionAssignments
            .AsNoTracking()
            .Where(a => a.DeletedAt == null);

        var baseQuery =
            from a in activeAssignments
            join d in _context.LessRangerDailyDivisions.AsNoTracking() on a.LessRangerDailyDivisionId equals d.Id
            join s in _context.LessRangerStations.AsNoTracking() on d.LessRangerStationId equals s.Id
            join z0 in _context.LessOperationalZones.AsNoTracking() on s.LessOperationalZoneId equals z0.Id into zg
            from z in zg.DefaultIfEmpty()
            join f in _context.LessRangerDutyFieldDefinitions.AsNoTracking() on a.DutyFieldDefinitionId equals f.Id
            where (scope.ParkId.HasValue && d.ParkId == scope.ParkId.Value)
                  || (scope.OfficeId.HasValue && d.OfficeId == scope.OfficeId.Value)
            select new
            {
                d.DutyDate,
                d.Category,
                ZoneId = z != null ? z.Id : Guid.Empty,
                ZoneName = z != null ? z.Name : string.Empty,
                StationId = s.Id,
                StationName = s.Name,
                FieldId = f.Id,
                FieldLabel = f.Label,
            };

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.DutyDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.DutyDate <= toDate.Value);

        if (!string.IsNullOrWhiteSpace(categoryFilter))
            baseQuery = baseQuery.Where(x => x.Category == categoryFilter);

        if (scope.ParkId.HasValue && zoneId.HasValue)
            baseQuery = baseQuery.Where(x => x.ZoneId == zoneId.Value);

        if (stationId.HasValue)
            baseQuery = baseQuery.Where(x => x.StationId == stationId.Value);

        if (fieldId.HasValue)
            baseQuery = baseQuery.Where(x => x.FieldId == fieldId.Value);

        if (!string.IsNullOrWhiteSpace(request.q))
        {
            var q = request.q.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                (x.ZoneName ?? string.Empty).ToLower().Contains(q)
                || (x.StationName ?? string.Empty).ToLower().Contains(q)
                || (x.FieldLabel ?? string.Empty).ToLower().Contains(q)
                || (x.Category ?? string.Empty).ToLower().Contains(q)
            );
        }

        var grouped =
            from x in baseQuery
            group x by new
            {
                x.DutyDate,
                x.Category,
                x.ZoneId,
                x.ZoneName,
                x.StationId,
                x.StationName,
                x.FieldId,
                x.FieldLabel,
            }
            into g
            select new LessRangerDailyDivisionPerFieldReportRowDto
            {
                DutyDate = g.Key.DutyDate,
                Category = g.Key.Category,
                LessOperationalZoneId = g.Key.ZoneId,
                LessOperationalZoneName = g.Key.ZoneName,
                StationId = g.Key.StationId,
                StationName = g.Key.StationName,
                DutyFieldDefinitionId = g.Key.FieldId,
                DutyFieldLabel = g.Key.FieldLabel,
                TotalStaff = g.Count(),
            };

        var sortBy = (request.sortBy ?? string.Empty).Trim().ToLowerInvariant();
        grouped = (sortBy, request.sortDesc) switch
        {
            ("dutydate", true) => grouped.OrderByDescending(x => x.DutyDate).ThenBy(x => x.StationName).ThenBy(x => x.DutyFieldLabel),
            ("dutydate", false) => grouped.OrderBy(x => x.DutyDate).ThenBy(x => x.StationName).ThenBy(x => x.DutyFieldLabel),

            ("stationname", true) => grouped.OrderByDescending(x => x.StationName).ThenByDescending(x => x.DutyDate),
            ("stationname", false) => grouped.OrderBy(x => x.StationName).ThenByDescending(x => x.DutyDate),

            ("field", true) => grouped.OrderByDescending(x => x.DutyFieldLabel).ThenByDescending(x => x.DutyDate),
            ("field", false) => grouped.OrderBy(x => x.DutyFieldLabel).ThenByDescending(x => x.DutyDate),

            ("totalstaff", true) => grouped.OrderByDescending(x => x.TotalStaff).ThenByDescending(x => x.DutyDate),
            ("totalstaff", false) => grouped.OrderBy(x => x.TotalStaff).ThenByDescending(x => x.DutyDate),

            _ => grouped.OrderByDescending(x => x.DutyDate).ThenBy(x => x.StationName).ThenBy(x => x.DutyFieldLabel),
        };

        return await PagedList<LessRangerDailyDivisionPerFieldReportRowDto>.CreateAsync(grouped, request.page, request.pageSize);
    }

    public async Task<PagedList<LessRangerDailyDivisionPerStationReportRowDto>> GetPerStationReport(
        LessRangerDailyDivisionPerStationReportRequest request
    )
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

        var activeAssignments = _context.LessRangerDailyDivisionAssignments
            .AsNoTracking()
            .Where(a => a.DeletedAt == null);

        var baseQuery =
            from d in _context.LessRangerDailyDivisions.AsNoTracking()
            join s in _context.LessRangerStations.AsNoTracking() on d.LessRangerStationId equals s.Id
            join z0 in _context.LessOperationalZones.AsNoTracking() on s.LessOperationalZoneId equals z0.Id into zg
            from z in zg.DefaultIfEmpty()
            join a in activeAssignments on d.Id equals a.LessRangerDailyDivisionId into da
            where (scope.ParkId.HasValue && d.ParkId == scope.ParkId.Value)
                  || (scope.OfficeId.HasValue && d.OfficeId == scope.OfficeId.Value)
            select new
            {
                d.DutyDate,
                d.Category,
                ZoneId = z != null ? z.Id : Guid.Empty,
                ZoneName = z != null ? z.Name : string.Empty,
                StationId = s.Id,
                StationName = s.Name,
                AssignmentCount = da.Count(),
            };

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.DutyDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.DutyDate <= toDate.Value);

        if (scope.ParkId.HasValue && zoneId.HasValue)
            baseQuery = baseQuery.Where(x => x.ZoneId == zoneId.Value);

        if (stationId.HasValue)
            baseQuery = baseQuery.Where(x => x.StationId == stationId.Value);

        if (!string.IsNullOrWhiteSpace(request.q))
        {
            var q = request.q.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                (x.ZoneName ?? string.Empty).ToLower().Contains(q)
                || (x.StationName ?? string.Empty).ToLower().Contains(q)
            );
        }

        var groupedByStationDate =
            from x in baseQuery
            group x by new { x.ZoneId, x.ZoneName, x.StationId, x.StationName, x.DutyDate } into g
            select new
            {
                g.Key.ZoneId,
                g.Key.ZoneName,
                g.Key.StationId,
                g.Key.StationName,
                g.Key.DutyDate,
                HasAskari = g.Any(a => a.Category == "ranger"),
                HasOfficers = g.Any(a => a.Category == "officer"),
                TotalAskari = g.Where(a => a.Category == "ranger").Sum(a => a.AssignmentCount),
                TotalOfficers = g.Where(a => a.Category == "officer").Sum(a => a.AssignmentCount),
            };

        var stationAgg =
            from x in groupedByStationDate
            group x by new { x.ZoneId, x.ZoneName, x.StationId, x.StationName } into g
            select new LessRangerDailyDivisionPerStationReportRowDto
            {
                LessOperationalZoneId = g.Key.ZoneId,
                LessOperationalZoneName = g.Key.ZoneName,
                StationId = g.Key.StationId,
                StationName = g.Key.StationName,
                TotalAskari = g.Sum(a => a.TotalAskari),
                TotalOfficers = g.Sum(a => a.TotalOfficers),
                TotalDays = g.Count(),
                CompleteDays = g.Count(a => a.HasAskari && a.HasOfficers),
            };

        var sortBy = (request.sortBy ?? string.Empty).Trim().ToLowerInvariant();
        stationAgg = (sortBy, request.sortDesc) switch
        {
            ("stationname", true) => stationAgg.OrderByDescending(x => x.StationName),
            ("stationname", false) => stationAgg.OrderBy(x => x.StationName),

            ("zonename", true) => stationAgg.OrderByDescending(x => x.LessOperationalZoneName).ThenBy(x => x.StationName),
            ("zonename", false) => stationAgg.OrderBy(x => x.LessOperationalZoneName).ThenBy(x => x.StationName),

            ("totalaskari", true) => stationAgg.OrderByDescending(x => x.TotalAskari),
            ("totalaskari", false) => stationAgg.OrderBy(x => x.TotalAskari),

            ("totalofficers", true) => stationAgg.OrderByDescending(x => x.TotalOfficers),
            ("totalofficers", false) => stationAgg.OrderBy(x => x.TotalOfficers),

            ("totaldays", true) => stationAgg.OrderByDescending(x => x.TotalDays),
            ("totaldays", false) => stationAgg.OrderBy(x => x.TotalDays),

            _ => stationAgg.OrderBy(x => x.LessOperationalZoneName).ThenBy(x => x.StationName),
        };

        return await PagedList<LessRangerDailyDivisionPerStationReportRowDto>.CreateAsync(stationAgg, request.page, request.pageSize);
    }

    public async Task<PagedList<LessRangerDailyDivisionCategorySummaryReportRowDto>> GetCategorySummaryReport(
        LessRangerDailyDivisionCategorySummaryReportRequest request
    )
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

        var activeAssignments = _context.LessRangerDailyDivisionAssignments
            .AsNoTracking()
            .Where(a => a.DeletedAt == null);

        var baseQuery =
            from d in _context.LessRangerDailyDivisions.AsNoTracking()
            join s in _context.LessRangerStations.AsNoTracking() on d.LessRangerStationId equals s.Id
            join z0 in _context.LessOperationalZones.AsNoTracking() on s.LessOperationalZoneId equals z0.Id into zg
            from z in zg.DefaultIfEmpty()
            join a in activeAssignments on d.Id equals a.LessRangerDailyDivisionId into da
            where (scope.ParkId.HasValue && d.ParkId == scope.ParkId.Value)
                  || (scope.OfficeId.HasValue && d.OfficeId == scope.OfficeId.Value)
            select new
            {
                d.DutyDate,
                d.Category,
                ZoneId = z != null ? z.Id : Guid.Empty,
                ZoneName = z != null ? z.Name : string.Empty,
                StationId = s.Id,
                StationName = s.Name,
                AssignmentCount = da.Count(),
            };

        if (fromDate.HasValue)
            baseQuery = baseQuery.Where(x => x.DutyDate >= fromDate.Value);

        if (toDate.HasValue)
            baseQuery = baseQuery.Where(x => x.DutyDate <= toDate.Value);

        if (scope.ParkId.HasValue && zoneId.HasValue)
            baseQuery = baseQuery.Where(x => x.ZoneId == zoneId.Value);

        if (stationId.HasValue)
            baseQuery = baseQuery.Where(x => x.StationId == stationId.Value);

        if (!string.IsNullOrWhiteSpace(request.q))
        {
            var q = request.q.Trim().ToLower();
            baseQuery = baseQuery.Where(x =>
                (x.ZoneName ?? string.Empty).ToLower().Contains(q)
                || (x.StationName ?? string.Empty).ToLower().Contains(q)
            );
        }

        var grouped =
            from x in baseQuery
            group x by x.DutyDate into g
            select new LessRangerDailyDivisionCategorySummaryReportRowDto
            {
                DutyDate = g.Key,
                TotalAskari = g.Where(a => a.Category == "ranger").Sum(a => a.AssignmentCount),
                TotalOfficers = g.Where(a => a.Category == "officer").Sum(a => a.AssignmentCount),
            };

        var sortBy = (request.sortBy ?? string.Empty).Trim().ToLowerInvariant();
        grouped = (sortBy, request.sortDesc) switch
        {
            ("dutydate", false) => grouped.OrderBy(x => x.DutyDate),
            ("totalaskari", true) => grouped.OrderByDescending(x => x.TotalAskari).ThenByDescending(x => x.DutyDate),
            ("totalaskari", false) => grouped.OrderBy(x => x.TotalAskari).ThenByDescending(x => x.DutyDate),
            ("totalofficers", true) => grouped.OrderByDescending(x => x.TotalOfficers).ThenByDescending(x => x.DutyDate),
            ("totalofficers", false) => grouped.OrderBy(x => x.TotalOfficers).ThenByDescending(x => x.DutyDate),
            _ => grouped.OrderByDescending(x => x.DutyDate),
        };

        return await PagedList<LessRangerDailyDivisionCategorySummaryReportRowDto>.CreateAsync(grouped, request.page, request.pageSize);
    }

    private string NormalizeCategoryOrThrow(string? category)
    {
        var v = (category ?? string.Empty).Trim().ToLowerInvariant();
        if (v is "officer" or "ranger") return v;
        throw new InvalidOperationException("Category must be 'officer' or 'ranger'");
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

    private async Task<List<StaffOptionDto>> GetStationRoster(LessScope scope, Guid stationId, DateOnly dutyDate, string category)
    {
        var start = dutyDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end = dutyDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        var postings = _context.LessStaffPostings
            .AsNoTracking()
            .Where(p => p.LessRangerStationId == stationId)
            .Where(p => p.EffectiveFrom <= end)
            .Where(p => p.EffectiveTo == null || p.EffectiveTo >= start);

        if (scope.ParkId.HasValue)
            postings = postings.Where(p => p.ParkId == scope.ParkId.Value);
        else if (scope.OfficeId.HasValue)
            postings = postings.Where(p => p.OfficeId == scope.OfficeId.Value);
        else
            return new List<StaffOptionDto>();

        var staffIds = await postings
            .Select(p => p.StaffId)
            .Distinct()
            .ToListAsync();

        var roster = await _context.Staffs
            .AsNoTracking()
            .Include(s => s.Rank)
            .Where(s => staffIds.Contains(s.Id))
            .ToListAsync();

        var rankIds = roster.Select(s => s.RankId).Distinct().ToList();
        var rankCategory = await _context.LessRangerRankCategories
            .AsNoTracking()
            .Where(x => rankIds.Contains(x.RankId))
            .ToDictionaryAsync(x => x.RankId, x => x.Category);

        return roster
            .Where(s => rankCategory.TryGetValue(s.RankId, out var c) && c == category)
            .Select(s => new StaffOptionDto
            {
                Id = s.Id,
                Name = s.FirstName + " " + s.LastName,
                RankId = s.RankId,
                RankName = s.Rank.Name,
            })
            .OrderBy(x => x.Name)
            .ToList();
    }
}
