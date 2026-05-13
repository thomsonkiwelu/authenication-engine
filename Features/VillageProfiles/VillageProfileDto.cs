using conservation_backend.Features.Files;
using conservation_backend.Features.GovernmentLeaders;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Features.VillageContexts;
using conservation_backend.Features.Villages;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.VillageProfiles;

public record VillageProfilePaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
}

public record VillageProfileDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public Guid VillageId { get; set; }
    public float VillageSize { get; set; }
    public float NumberOfHousehold { get; set; }
    public float NumberOfMale { get; set; }
    public float NumberOfFemale { get; set; }
    public float NumberOfCow { get; set; }
    public float NumberOfSheep { get; set; }
    public float NumberOfGoat { get; set; }
    public float NumberOfDog { get; set; }
    public float Population { get; set; }
    public string LandStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public VillageDto Village { get; set; } = new VillageDto();
    public FileDto? File { get; set; } = new FileDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record VillageProfileRequestDto
{
    public Guid ParkId { get; set; }
    public Guid VillageId { get; set; }
    public float VillageSize { get; set; }
    public float NumberOfHousehold { get; set; }
    public float NumberOfMale { get; set; }
    public float NumberOfFemale { get; set; }
    public float NumberOfCow { get; set; }
    public float NumberOfSheep { get; set; }
    public float NumberOfGoat { get; set; }
    public float NumberOfDog { get; set; }
    public float Population { get; set; }
    public string LandStatus { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record VillageProfileResponseDto : VillageProfileDto
{
    public int RowNumber { get; set; }
}

public record GetVillageProfileDto : VillageProfileDto
{
    public List<GovernmentLeaderDto> GovernmentLeaders { get; set; } = new List<GovernmentLeaderDto>();
    public List<VillageContextDto> LandUsePractices { get; set; } = new List<VillageContextDto>();
    public List<VillageContextDto> VillageFacilities { get; set; } = new List<VillageContextDto>();
    public List<VillageContextDto> CommunityOrganizations { get; set; } = new List<VillageContextDto>();
    public List<VillageContextDto> CommunityProjects { get; set; } = new List<VillageContextDto>();
    public List<NonGovernmentOrganizationDto> NonGovernmentOrganizations { get; set; } = new List<NonGovernmentOrganizationDto>();
    //VillageIssues
    public List<CommunitySelectionDto> ParkRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> LandRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> AnimalHusbandryRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> FoodSupplyAndSecurityProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> SecurityRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
    public List<CommunitySelectionDto> LeadershipRelatedProblems { get; set; } = new List<CommunitySelectionDto>();
}

public record NonGovernmentOrganizationDto : VillageContextDto
{
    public List<CommunitySelectionDto> Identifications { get; set; } = new List<CommunitySelectionDto>();
}