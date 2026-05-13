using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.VillageContexts;

public record VillageContextPaginationDto : PaginationDto
{
    public string? EntityId { get; set; }
    public string? EntityName { get; set; }
    public string? FieldName { get; set; }
}

public record VillageContextDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
    public List<CommunitySelectionDto> Identifications { get; set; } = new List<CommunitySelectionDto>();
}

public record VillageContextRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public List<string>? Identifications { get; set; } = new List<string>();
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record VillageContextResponseDto : VillageContextDto
{
    public int RowNumber { get; set; }
}

public record VillageIssueRequestDto
{
    public string? OtherParkRelatedProblem { get; set; } = string.Empty;
    public string? OtherLandRelatedProblem { get; set; } = string.Empty;
    public string? OtherAnimalHusbandryRelatedProblem { get; set; } = string.Empty;
    public string? OtherFoodSupplyAndSecurityProblem { get; set; } = string.Empty;
    public string? OtherSecurityRelatedProblem { get; set; } = string.Empty;
    public string? OtherLeadershipRelatedProblem { get; set; } = string.Empty;
    public List<string> ParkRelatedProblems { get; set; } = new List<string>();
    public List<string> LandRelatedProblems { get; set; } = new List<string>();
    public List<string> AnimalHusbandryRelatedProblems { get; set; } = new List<string>();
    public List<string> FoodSupplyAndSecurityProblems { get; set; } = new List<string>();
    public List<string> SecurityRelatedProblems { get; set; } = new List<string>();
    public List<string> LeadershipRelatedProblems { get; set; } = new List<string>();
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record GetVillageIssueDto
{
    public List<CommunitySelectionDto> ParkRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> LandRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> AnimalHusbandryRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> FoodSupplyAndSecurityProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> SecurityRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> LeadershipRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
}