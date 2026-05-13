namespace authentication_engine.Features.SystemModules;

public record SystemModuleDto()
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;
}