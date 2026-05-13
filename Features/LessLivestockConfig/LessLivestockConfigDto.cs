namespace conservation_backend.Features.LessLivestockConfig;

public record LessLivestockTypeDto
{
    public Guid? Id { get; init; }

    public string Key { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public bool IsActive { get; init; } = true;
}

public record LessLivestockActionOptionDto
{
    public Guid? Id { get; init; }

    public string Key { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public bool RequiresControlNumber { get; init; }

    public bool RequiresCaseNumber { get; init; }

    public bool IsRevenue { get; init; }

    public bool IsActive { get; init; } = true;
}

public record LessLivestockConfigResponseDto
{
    public List<LessLivestockTypeDto> LivestockTypes { get; init; } = new();

    public List<LessLivestockActionOptionDto> ActionOptions { get; init; } = new();
}

public record LessLivestockConfigUpdateRequest
{
    public List<LessLivestockTypeDto> LivestockTypes { get; init; } = new();

    public List<LessLivestockActionOptionDto> ActionOptions { get; init; } = new();
}
