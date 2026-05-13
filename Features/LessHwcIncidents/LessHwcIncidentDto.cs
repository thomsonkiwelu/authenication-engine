using conservation_backend.Shared;

namespace conservation_backend.Features.LessHwcIncidents;

public record LessHwcIncidentHeaderDto
{
    public Guid Id { get; init; }

    public Guid ParkId { get; init; }

    public string ParkName { get; init; } = string.Empty;

    public Guid? OfficeId { get; init; }

    public DateOnly IncidentDate { get; init; }

    public string District { get; init; } = string.Empty;

    public string Ward { get; init; } = string.Empty;

    public string Villages { get; init; } = string.Empty;

    public string IncidentCategoryKey { get; init; } = string.Empty;

    public int TotalIncidents { get; init; }

    public decimal EstimatedLossTzs { get; init; }

    public string ReferenceNo { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public string Status { get; init; } = "Reported";
}

public record LessHwcIncidentResponseDto
{
    public Guid Id { get; init; }

    public Guid ParkId { get; init; }

    public Guid? OfficeId { get; init; }

    public DateOnly IncidentDate { get; init; }

    public string Status { get; init; } = "Reported";

    public Dictionary<string, string?> Data { get; init; } = new();

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}

public record LessHwcIncidentCreateRequest
{
    public string? ParkId { get; init; } // required for office-scoped users

    public string IncidentDate { get; init; } = string.Empty; // yyyy-MM-dd

    public string? Phase { get; init; } // report, response, followup

    public Dictionary<string, string?> Data { get; init; } = new();
}

public record LessHwcIncidentUpdateRequest
{
    public string? ParkId { get; init; } // optional; required for office-scoped users if record has no park

    public string IncidentDate { get; init; } = string.Empty; // yyyy-MM-dd

    public string? Phase { get; init; } // report, response, followup

    public Dictionary<string, string?> Data { get; init; } = new();
}

public record LessHwcIncidentHeadersRequest : PaginationDto
{
    public string? FromDate { get; init; } // yyyy-MM-dd

    public string? ToDate { get; init; } // yyyy-MM-dd

    public string? ParkId { get; init; }

    public string? IncidentCategoryKey { get; init; }
}
