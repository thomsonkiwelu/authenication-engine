using conservation_backend.Shared;

namespace conservation_backend.Features.LessActivities;

public record LessActivityPaginationDto : PaginationDto
{
    public string? Category { get; set; } = string.Empty;
}

public record LessActivityDto
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record LessActivityRequest
{
    public string Category { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public record LessActivityResponseDto : LessActivityDto
{
    public int RowNumber { get; set; }
}
