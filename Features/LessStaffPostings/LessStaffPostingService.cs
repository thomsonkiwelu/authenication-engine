using conservation_backend.Config;
using conservation_backend.Features.LessOperationalZones;
using conservation_backend.Features.LessRangerGroups;
using conservation_backend.Features.LessRangerStations;
using conservation_backend.Features.LessStaffPostings.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace conservation_backend.Features.LessStaffPostings;

public class LessStaffPostingService(
    AppDBContext context,
    IUserContext userContext,
    ILessStaffPostingRepository repository,
    IMapper mapper
) : ILessStaffPostingService
{
    private readonly AppDBContext _context = context;
    private readonly IUserContext _userContext = userContext;
    private readonly ILessStaffPostingRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedList<LessStaffPostingDto>> GetPostings(LessStaffPostingPaginationDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.OfficeId))
        {
            var officeId = await GetOfficeIdOrThrow(dto.OfficeId);
            dto = dto with { OfficeId = officeId.ToString(), ParkId = null };
        }
        else
        {
            var parkId = GetParkIdOrThrow(dto.ParkId);
            dto = dto with { ParkId = parkId.ToString() };
        }

        var paged = await _repository.GetPagedData(dto);

        var dtoList = paged.Data.Select(p => new LessStaffPostingDto
        {
            Id = p.Id,
            StaffId = p.StaffId,
            StaffName = p.Staff.FirstName + " " + p.Staff.LastName,
            RankCategory = p.Staff.Rank.Category,
            ParkId = p.ParkId,
            ParkName = p.Park == null ? null : p.Park.Name,
            LessOperationalZoneId = p.LessOperationalZoneId,
            LessOperationalZoneName = p.LessOperationalZone == null ? null : p.LessOperationalZone.Name,
            OfficeId = p.OfficeId,
            OfficeName = p.Office == null ? null : p.Office.Name,
            LessRangerStationId = p.LessRangerStationId,
            LessRangerStationName = p.LessRangerStation == null ? null : p.LessRangerStation.Name,
            LessRangerGroupId = p.LessRangerGroupId,
            LessRangerGroupName = p.LessRangerGroup == null ? null : p.LessRangerGroup.Name,
            EffectiveFrom = p.EffectiveFrom,
            EffectiveTo = p.EffectiveTo,
            Remarks = p.Remarks,
        }).ToList();

        return new PagedList<LessStaffPostingDto>(
            items: dtoList,
            page: paged.Page,
            pageSize: paged.PageSize,
            totalCount: paged.TotalCount
        );
    }

    public async Task<LessStaffPostingDto> Assign(LessStaffPostingAssignRequest request)
    {
        var staffId = Guid.Parse(request.StaffId);

        var isOfficeScope = !string.IsNullOrWhiteSpace(request.OfficeId);
        var stationId = string.IsNullOrWhiteSpace(request.LessRangerStationId)
            ? (Guid?)null
            : Guid.Parse(request.LessRangerStationId);
        var groupId = string.IsNullOrWhiteSpace(request.LessRangerGroupId)
            ? (Guid?)null
            : Guid.Parse(request.LessRangerGroupId);

        Guid? officeId = null;
        Guid? parkId = null;
        Guid? zoneId = null;

        if (isOfficeScope)
        {
            officeId = await GetOfficeIdOrThrow(request.OfficeId);
            if (!stationId.HasValue)
                throw new InvalidOperationException("Station is required for office-scoped posting");

            await ValidateOfficeHierarchy(officeId.Value, stationId.Value, groupId);
        }
        else
        {
            parkId = GetParkIdOrThrow(request.ParkId);
            if (string.IsNullOrWhiteSpace(request.LessOperationalZoneId))
                throw new InvalidOperationException("Operational zone is required for park-scoped posting");

            zoneId = Guid.Parse(request.LessOperationalZoneId);
            await ValidateHierarchy(parkId.Value, zoneId.Value, stationId, groupId);
        }

        var active = await _repository.GetActivePostingByStaffId(staffId);
        if (active != null)
        {
            var sameDestination = active.OfficeId == officeId
                                  && active.ParkId == parkId
                                  && active.LessOperationalZoneId == zoneId
                                  && active.LessRangerStationId == stationId
                                  && active.LessRangerGroupId == groupId;

            if (sameDestination)
            {
                var loadedExisting = await _context.LessStaffPostings
                    .Include(p => p.Staff)
                    .Include(p => p.Park)
                    .Include(p => p.LessOperationalZone)
                    .Include(p => p.Office)
                    .Include(p => p.LessRangerStation)
                    .Include(p => p.LessRangerGroup)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == active.Id);

                if (loadedExisting is null)
                    throw new KeyNotFoundException("Failed to load existing posting");

                return new LessStaffPostingDto
                {
                    Id = loadedExisting.Id,
                    StaffId = loadedExisting.StaffId,
                    StaffName = loadedExisting.Staff.FirstName + " " + loadedExisting.Staff.LastName,
                    ParkId = loadedExisting.ParkId,
                    ParkName = loadedExisting.Park == null ? null : loadedExisting.Park.Name,
                    LessOperationalZoneId = loadedExisting.LessOperationalZoneId,
                    LessOperationalZoneName = loadedExisting.LessOperationalZone == null ? null : loadedExisting.LessOperationalZone.Name,
                    OfficeId = loadedExisting.OfficeId,
                    OfficeName = loadedExisting.Office == null ? null : loadedExisting.Office.Name,
                    LessRangerStationId = loadedExisting.LessRangerStationId,
                    LessRangerStationName = loadedExisting.LessRangerStation == null ? null : loadedExisting.LessRangerStation.Name,
                    LessRangerGroupId = loadedExisting.LessRangerGroupId,
                    LessRangerGroupName = loadedExisting.LessRangerGroup == null ? null : loadedExisting.LessRangerGroup.Name,
                    EffectiveFrom = loadedExisting.EffectiveFrom,
                    EffectiveTo = loadedExisting.EffectiveTo,
                    Remarks = loadedExisting.Remarks,
                };
            }

            throw new InvalidOperationException("Staff is already assigned. Unassign first before assigning to another station.");
        }

        var posting = new LessStaffPosting
        {
            Id = Guid.NewGuid(),
            StaffId = staffId,
            ParkId = parkId,
            LessOperationalZoneId = zoneId,
            OfficeId = officeId,
            LessRangerStationId = stationId,
            LessRangerGroupId = groupId,
            EffectiveFrom = DateTime.UtcNow,
            EffectiveTo = null,
            Remarks = request.Remarks
        };

        var created = await _repository.Create(posting);

        var loaded = await _context.LessStaffPostings
            .Include(p => p.Staff)
            .Include(p => p.Park)
            .Include(p => p.LessOperationalZone)
            .Include(p => p.Office)
            .Include(p => p.LessRangerStation)
            .Include(p => p.LessRangerGroup)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == created.Id);

        if (loaded is null)
            throw new KeyNotFoundException("Failed to load created posting");

        return new LessStaffPostingDto
        {
            Id = loaded.Id,
            StaffId = loaded.StaffId,
            StaffName = loaded.Staff.FirstName + " " + loaded.Staff.LastName,
            ParkId = loaded.ParkId,
            ParkName = loaded.Park == null ? null : loaded.Park.Name,
            LessOperationalZoneId = loaded.LessOperationalZoneId,
            LessOperationalZoneName = loaded.LessOperationalZone == null ? null : loaded.LessOperationalZone.Name,
            OfficeId = loaded.OfficeId,
            OfficeName = loaded.Office == null ? null : loaded.Office.Name,
            LessRangerStationId = loaded.LessRangerStationId,
            LessRangerStationName = loaded.LessRangerStation == null ? null : loaded.LessRangerStation.Name,
            LessRangerGroupId = loaded.LessRangerGroupId,
            LessRangerGroupName = loaded.LessRangerGroup == null ? null : loaded.LessRangerGroup.Name,
            EffectiveFrom = loaded.EffectiveFrom,
            EffectiveTo = loaded.EffectiveTo,
            Remarks = loaded.Remarks,
        };
    }

    public async Task<List<LessStaffPostingDto>> BulkAssign(LessStaffPostingBulkAssignRequest request)
    {
        var stationId = string.IsNullOrWhiteSpace(request.LessRangerStationId)
            ? (Guid?)null
            : Guid.Parse(request.LessRangerStationId);
        var groupId = string.IsNullOrWhiteSpace(request.LessRangerGroupId)
            ? (Guid?)null
            : Guid.Parse(request.LessRangerGroupId);

        var isOfficeScope = !string.IsNullOrWhiteSpace(request.OfficeId);

        Guid? officeId = null;
        Guid? parkId = null;
        Guid? zoneId = null;

        if (isOfficeScope)
        {
            officeId = await GetOfficeIdOrThrow(request.OfficeId);
            if (!stationId.HasValue)
                throw new InvalidOperationException("Station is required for office-scoped posting");

            await ValidateOfficeHierarchy(officeId.Value, stationId.Value, groupId);
        }
        else
        {
            parkId = GetParkIdOrThrow(request.ParkId);
            if (string.IsNullOrWhiteSpace(request.LessOperationalZoneId))
                throw new InvalidOperationException("Operational zone is required for park-scoped posting");

            zoneId = Guid.Parse(request.LessOperationalZoneId);
            await ValidateHierarchy(parkId.Value, zoneId.Value, stationId, groupId);
        }

        if (request.StaffIds is null || request.StaffIds.Count == 0)
            return new List<LessStaffPostingDto>();

        var staffIds = request.StaffIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(Guid.Parse)
            .Distinct()
            .ToList();

        if (staffIds.Count == 0)
            return new List<LessStaffPostingDto>();

        var activePostings = await _context.LessStaffPostings
            .AsNoTracking()
            .Where(p => p.EffectiveTo == null)
            .Where(p => staffIds.Contains(p.StaffId))
            .Select(p => new
            {
                p.Id,
                p.StaffId,
                p.OfficeId,
                p.ParkId,
                p.LessOperationalZoneId,
                p.LessRangerStationId,
                p.LessRangerGroupId
            })
            .ToListAsync();

        var activeMap = activePostings.ToDictionary(x => x.StaffId, x => x);

        var notAllowed = activePostings
            .Where(x => !(x.OfficeId == officeId
                          && x.ParkId == parkId
                          && x.LessOperationalZoneId == zoneId
                          && x.LessRangerStationId == stationId
                          && x.LessRangerGroupId == groupId))
            .Select(x => x.StaffId)
            .ToList();

        if (notAllowed.Any())
            throw new InvalidOperationException("One or more selected staff are already assigned. Unassign them first before assigning to another station.");

        await using var tx = await _context.Database.BeginTransactionAsync();

        var now = DateTime.UtcNow;
        var createdIds = new List<Guid>(staffIds.Count);

        foreach (var staffId in staffIds)
        {
            if (activeMap.ContainsKey(staffId))
                continue;

            var posting = new LessStaffPosting
            {
                Id = Guid.NewGuid(),
                StaffId = staffId,
                ParkId = parkId,
                LessOperationalZoneId = zoneId,
                OfficeId = officeId,
                LessRangerStationId = stationId,
                LessRangerGroupId = groupId,
                EffectiveFrom = now,
                EffectiveTo = null,
                Remarks = request.Remarks
            };

            var created = await _repository.Create(posting);
            createdIds.Add(created.Id);
        }

        await tx.CommitAsync();

        var loaded = await _context.LessStaffPostings
            .Include(p => p.Staff)
            .Include(p => p.Park)
            .Include(p => p.LessOperationalZone)
            .Include(p => p.Office)
            .Include(p => p.LessRangerStation)
            .Include(p => p.LessRangerGroup)
            .AsNoTracking()
            .Where(p => createdIds.Contains(p.Id))
            .ToListAsync();

        return loaded
            .Select(p => new LessStaffPostingDto
            {
                Id = p.Id,
                StaffId = p.StaffId,
                StaffName = p.Staff.FirstName + " " + p.Staff.LastName,
                ParkId = p.ParkId,
                ParkName = p.Park == null ? null : p.Park.Name,
                LessOperationalZoneId = p.LessOperationalZoneId,
                LessOperationalZoneName = p.LessOperationalZone == null ? null : p.LessOperationalZone.Name,
                OfficeId = p.OfficeId,
                OfficeName = p.Office == null ? null : p.Office.Name,
                LessRangerStationId = p.LessRangerStationId,
                LessRangerStationName = p.LessRangerStation == null ? null : p.LessRangerStation.Name,
                LessRangerGroupId = p.LessRangerGroupId,
                LessRangerGroupName = p.LessRangerGroup == null ? null : p.LessRangerGroup.Name,
                EffectiveFrom = p.EffectiveFrom,
                EffectiveTo = p.EffectiveTo,
                Remarks = p.Remarks,
            })
            .OrderBy(x => x.StaffName)
            .ToList();
    }

    public async Task<bool> Unassign(LessStaffPostingUnassignRequest request)
    {
        var staffId = Guid.Parse(request.StaffId);

        // Close active posting (any park)
        await _repository.CloseActivePostingByStaffId(staffId, request.Remarks);
        return true;
    }

    public async Task<List<StaffOptionDto>> GetParkStaffOptions(string parkId)
    {
        var pid = GetParkIdOrThrow(parkId);
        return await _repository.GetParkStaffOptions(pid);
    }

    public async Task<List<StaffOptionDto>> GetOfficeStaffOptions(string officeId)
    {
        var oid = await GetOfficeIdOrThrow(officeId);
        return await _repository.GetOfficeStaffOptions(oid);
    }

    private Guid GetParkIdOrThrow(string? requestParkId)
    {
        var authorizedParks = _userContext.GetAuthorizedParkIds(_context);

        if (authorizedParks.Count != 1)
            throw new UnauthorizedAccessException("This action is only allowed for park-scoped users.");

        var parkId = authorizedParks[0];

        if (!string.IsNullOrWhiteSpace(requestParkId) && Guid.TryParse(requestParkId, out var parsed))
        {
            if (parsed != parkId)
                throw new UnauthorizedAccessException("You are not authorized to access this park.");
        }

        return parkId;
    }

    private async Task<Guid> GetOfficeIdOrThrow(string? requestOfficeId)
    {
        if (string.IsNullOrWhiteSpace(requestOfficeId) || !Guid.TryParse(requestOfficeId, out var requested))
            throw new InvalidOperationException("Office is required for office-scoped actions");

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
            throw new UnauthorizedAccessException("This action is only allowed for office-scoped users.");

        var officeId = officeIds[0];

        if (requested != officeId)
            throw new UnauthorizedAccessException("You are not authorized to access this office.");

        return officeId;
    }

    private async Task ValidateHierarchy(Guid parkId, Guid zoneId, Guid? stationId, Guid? groupId)
    {
        var zone = await _context.LessOperationalZones
            .AsNoTracking()
            .FirstOrDefaultAsync(z => z.Id == zoneId);

        if (zone is null)
            throw new KeyNotFoundException("Operational zone not found");

        if (zone.ParkId != parkId)
            throw new InvalidOperationException("Zone does not belong to the specified park");

        if (stationId.HasValue)
        {
            var station = await _context.LessRangerStations
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == stationId.Value);

            if (station is null)
                throw new KeyNotFoundException("Ranger station not found");

            if (station.LessOperationalZoneId != zoneId)
                throw new InvalidOperationException("Station does not belong to the specified zone");
        }

        if (groupId.HasValue)
        {
            if (!stationId.HasValue)
                throw new InvalidOperationException("Group cannot be selected without station");

            var group = await _context.LessRangerGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == groupId.Value);

            if (group is null)
                throw new KeyNotFoundException("Ranger group not found");

            if (group.LessRangerStationId != stationId.Value)
                throw new InvalidOperationException("Group does not belong to the specified station");
        }
    }

    private async Task ValidateOfficeHierarchy(Guid officeId, Guid stationId, Guid? groupId)
    {
        var station = await _context.LessRangerStations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == stationId);

        if (station is null)
            throw new KeyNotFoundException("Ranger station not found");

        if (station.OfficeId != officeId)
            throw new InvalidOperationException("Station does not belong to the specified office");

        if (groupId.HasValue)
        {
            var group = await _context.LessRangerGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == groupId.Value);

            if (group is null)
                throw new KeyNotFoundException("Ranger group not found");

            if (group.LessRangerStationId != stationId)
                throw new InvalidOperationException("Group does not belong to the specified station");
        }
    }
}
