using conservation_backend.Features.LessStaffPostings;
using conservation_backend.Shared;

namespace conservation_backend.Features.LessRangerDailyDivisions;

public record LessRangerDailyDivisionFieldAssignmentDto
{
    public Guid DutyFieldDefinitionId { get; init; }

    public string Key { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public List<StaffOptionDto> Staff { get; init; } = new();

    public int Count => Staff.Count;
}

public record LessRangerDailyDivisionResponseDto
{
    public Guid? DivisionId { get; init; }

    public Guid? ParkId { get; init; }

    public Guid? OfficeId { get; init; }

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public DateOnly DutyDate { get; init; }

    public string Category { get; init; } = string.Empty; // officer | ranger

    public List<StaffOptionDto> StationRoster { get; init; } = new();

    public List<LessRangerDailyDivisionFieldAssignmentDto> Fields { get; init; } = new();
}

public record LessRangerDailyDivisionGetRequest
{
    public string StationId { get; init; } = string.Empty;

    public string DutyDate { get; init; } = string.Empty; // yyyy-MM-dd

    public string Category { get; init; } = string.Empty; // officer | ranger
}

public record LessRangerDailyDivisionSaveFieldRequest
{
    public string DutyFieldDefinitionId { get; init; } = string.Empty;

    public List<string> StaffIds { get; init; } = new();
}

public record LessRangerDailyDivisionSaveRequest
{
    public string StationId { get; init; } = string.Empty;

    public string DutyDate { get; init; } = string.Empty; // yyyy-MM-dd

    public string Category { get; init; } = string.Empty; // officer | ranger

    public List<LessRangerDailyDivisionSaveFieldRequest> Fields { get; init; } = new();
}

public record LessRangerDailyDivisionHeadersRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? Category { get; init; } // officer | ranger

    public string? LessOperationalZoneId { get; init; }

    public string? StationId { get; init; }
}

public record LessRangerDailyDivisionHeaderDto
{
    public DateOnly DutyDate { get; init; }

    public Guid? LessOperationalZoneId { get; init; }

    public string LessOperationalZoneName { get; init; } = string.Empty;

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public int TotalAskari { get; init; }

    public int TotalOfficers { get; init; }

    public bool HasAskariDivision { get; init; }

    public bool HasOfficerDivision { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Status { get; init; } = string.Empty;
}

public record LessRangerDailyDivisionPerFieldReportRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? Category { get; init; } // officer | ranger

    public string? LessOperationalZoneId { get; init; }

    public string? StationId { get; init; }

    public string? DutyFieldDefinitionId { get; init; }
}

public record LessRangerDailyDivisionPerFieldReportRowDto
{
    public DateOnly DutyDate { get; init; }

    public string Category { get; init; } = string.Empty;

    public Guid LessOperationalZoneId { get; init; }

    public string LessOperationalZoneName { get; init; } = string.Empty;

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public Guid DutyFieldDefinitionId { get; init; }

    public string DutyFieldLabel { get; init; } = string.Empty;

    public int TotalStaff { get; init; }
}

public record LessRangerDailyDivisionPerStationReportRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? LessOperationalZoneId { get; init; }

    public string? StationId { get; init; }
}

public record LessRangerDailyDivisionPerStationReportRowDto
{
    public Guid LessOperationalZoneId { get; init; }

    public string LessOperationalZoneName { get; init; } = string.Empty;

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public int TotalAskari { get; init; }

    public int TotalOfficers { get; init; }

    public int TotalDays { get; init; }

    public int CompleteDays { get; init; }
}

public record LessRangerDailyDivisionCategorySummaryReportRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? LessOperationalZoneId { get; init; }

    public string? StationId { get; init; }
}

public record LessRangerDailyDivisionCategorySummaryReportRowDto
{
    public DateOnly DutyDate { get; init; }

    public int TotalAskari { get; init; }

    public int TotalOfficers { get; init; }
}
