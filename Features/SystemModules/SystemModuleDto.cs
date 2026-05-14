using authentication_engine.Features.SystemApplications;
using authentication_engine.Features.Users;
using authentication_engine.Shared;

namespace authentication_engine.Features.SystemModules;

public record SystemModulePaginationDto : PaginationDto
{
    public string? SystemApplicationId { get; init; }
}

public record SystemModuleDto()
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string SystemApplicationId { get; init; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public SystemApplicationDto SystemApplication { get; set; } = new SystemApplicationDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record SystemModuleRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string SystemApplicationId { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}
    
public record SystemModuleResponseDto: SystemModuleDto
{
    public int RowNumber { get; set; }
}

public record SystemModuleMinimalDto()
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}