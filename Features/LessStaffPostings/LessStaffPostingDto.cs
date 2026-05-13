using authentication_engine.Shared;

namespace authentication_engine.Features.LessStaffPostings;

public record LessStaffPostingAssignRequest(
    string StaffId,
    string? ParkId,
    string? LessOperationalZoneId,
    string? OfficeId,
    string? LessRangerStationId,
    string? LessRangerGroupId,
    string? Remarks
);

public record LessStaffPostingBulkAssignRequest(
    List<string> StaffIds,
    string? ParkId,
    string? LessOperationalZoneId,
    string? OfficeId,
    string? LessRangerStationId,
    string? LessRangerGroupId,
    string? Remarks
);

public record LessStaffPostingUnassignRequest(
    string StaffId,
    string? Remarks
);

public record StaffOptionDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid RankId { get; init; }

    public string RankName { get; init; } = string.Empty;
}

public record LessStaffPostingDto
{
    public Guid Id { get; init; }

    public Guid StaffId { get; init; }

    public string StaffName { get; init; } = string.Empty;
    
    public string RankCategory { get; init; } = string.Empty;

    public Guid? ParkId { get; init; }

    public string? ParkName { get; init; }

    public Guid? LessOperationalZoneId { get; init; }

    public string? LessOperationalZoneName { get; init; }

    public Guid? OfficeId { get; init; }

    public string? OfficeName { get; init; }

    public Guid? LessRangerStationId { get; init; }

    public string? LessRangerStationName { get; init; }

    public Guid? LessRangerGroupId { get; init; }

    public string? LessRangerGroupName { get; init; }

    public DateTime EffectiveFrom { get; init; }

    public DateTime? EffectiveTo { get; init; }

    public string? Remarks { get; init; }
}

public record LessStaffPostingPaginationDto : PaginationDto
{
    public string? ParkId { get; init; }

    public string? OfficeId { get; init; }

    public string? LessOperationalZoneId { get; init; }

    public string? LessRangerStationId { get; init; }

    public string? LessRangerGroupId { get; init; }

    public string? StaffId { get; init; }
}
