using conservation_backend.Shared;

namespace conservation_backend.Features.LessLivestockDailies;

public record LessLivestockDailyResponseDto
{
    public Guid? LivestockDailyId { get; init; }

    public Guid? ParkId { get; init; }

    public Guid? OfficeId { get; init; }

    public Guid? LessOperationalZoneId { get; init; }

    public string? LessOperationalZoneName { get; init; }

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public DateOnly DutyDate { get; init; }

    public List<LessLivestockDailyLivestockDto> Livestock { get; init; } = new();

    public List<LessLivestockDailyActionDto> Actions { get; init; } = new();

    public decimal TotalRevenueAmount { get; init; }
}

public record LessLivestockDailyLivestockDto
{
    public string LivestockTypeKey { get; init; } = string.Empty;

    public int Count { get; init; }
}

public record LessLivestockDailyActionDto
{
    public string LivestockTypeKey { get; init; } = string.Empty;

    public string ActionOptionKey { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public decimal Amount { get; init; }

    public string ControlNumber { get; init; } = string.Empty;

    public string CaseNumber { get; init; } = string.Empty;
}

public record LessLivestockDailyGetRequest
{
    public string StationId { get; init; } = string.Empty;

    public string DutyDate { get; init; } = string.Empty; // yyyy-MM-dd
}

public record LessLivestockDailySaveRequest
{
    public string StationId { get; init; } = string.Empty;

    public string DutyDate { get; init; } = string.Empty; // yyyy-MM-dd

    public List<LessLivestockDailyLivestockDto> Livestock { get; init; } = new();

    public List<LessLivestockDailyActionDto> Actions { get; init; } = new();
}

public record LessLivestockDailyHeadersRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? LessOperationalZoneId { get; init; }

    public string? StationId { get; init; }
}

public record LessLivestockDailyHeaderDto
{
    public DateOnly DutyDate { get; init; }

    public Guid? LessOperationalZoneId { get; init; }

    public string? LessOperationalZoneName { get; init; }

    public Guid StationId { get; init; }

    public string StationName { get; init; } = string.Empty;

    public int TotalInCustody { get; init; }

    public decimal TotalRevenueAmount { get; init; }

    public DateTime CreatedAt { get; init; }

    public string Status { get; init; } = string.Empty;
}
