namespace conservation_backend.Features.LessHwcConfig;

public record LessHwcTabDto
{
    public Guid? Id { get; init; }

    public string Key { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public bool IsActive { get; init; } = true;
}

public record LessHwcFieldOptionDto
{
    public Guid? Id { get; init; }

    public string Value { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public bool IsActive { get; init; } = true;
}

public record LessHwcFieldDto
{
    public Guid? Id { get; init; }

    public Guid? TabDefinitionId { get; init; }

    public string TabKey { get; init; } = string.Empty;

    public string Key { get; init; } = string.Empty;

    public string Label { get; init; } = string.Empty;

    public string FieldType { get; init; } = "text";

    public string Placeholder { get; init; } = string.Empty;

    public bool IsRequired { get; init; }

    public bool IsComputed { get; init; }

    public string ComputeFromKeys { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public bool IsActive { get; init; } = true;

    public List<LessHwcFieldOptionDto> Options { get; init; } = new();
}

public record LessHwcConfigResponseDto
{
    public List<LessHwcTabDto> Tabs { get; init; } = new();

    public List<LessHwcFieldDto> Fields { get; init; } = new();
}

public record LessHwcConfigUpdateRequest
{
    public List<LessHwcTabDto> Tabs { get; init; } = new();

    public List<LessHwcFieldDto> Fields { get; init; } = new();
}
