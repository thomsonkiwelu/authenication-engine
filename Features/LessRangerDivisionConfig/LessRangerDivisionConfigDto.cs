namespace conservation_backend.Features.LessRangerDivisionConfig;

public record LessRangerRankCategoryItemDto
{
    public Guid RankId { get; init; }

    public string Category { get; init; } = string.Empty;
}

public record LessRangerDutyFieldDefinitionDto
{
    public Guid? Id { get; init; }

    public string Category { get; init; } = string.Empty;

    public string Key { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public bool IsActive { get; init; } = true;
}

public record LessRangerDivisionConfigResponseDto
{
    public List<LessRangerRankCategoryItemDto> RankCategories { get; init; } = new();

    public List<LessRangerDutyFieldDefinitionDto> DutyFields { get; init; } = new();
}

public record LessRangerDivisionConfigUpdateRequest
{
    public List<LessRangerRankCategoryItemDto> RankCategories { get; init; } = new();

    public List<LessRangerDutyFieldDefinitionDto> DutyFields { get; init; } = new();
}
