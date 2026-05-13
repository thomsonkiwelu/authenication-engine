using conservation_backend.Features.DistrictContexts;
using conservation_backend.Features.Districts;
using conservation_backend.Features.Files;
using conservation_backend.Features.GovernmentLeaders;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Tribes;
using conservation_backend.Features.Users;
using conservation_backend.Features.Villages;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.DistrictProfiles;

public record DistrictProfilePaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
}

public record DistrictProfileDto
{
    public Guid Id { get; set; }
    public Guid ParkId { get; set; }
    public Guid DistrictId { get; set; }
    public float AreaSize { get; set; }
    public float Population { get; set; }
    public float PopulationGrowthRate { get; set; }
    public float AreaOccupiedByPark { get; set; }
    public string RelationshipStatus { get; set; } = string.Empty;
    public float AverageAnnualRainfall { get; set; }
    public string Landform { get; set; } = string.Empty;
    public string RainfallPattern { get; set; } = string.Empty;
    public string VegetationCharacteristics { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public DistrictDto District { get; set; } = new DistrictDto();
    public FileDto? File { get; set; } = new FileDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record DistrictProfileRequestDto
{
    public Guid ParkId { get; set; }
    public Guid DistrictId { get; set; }
    public float AreaSize { get; set; }
    public float Population { get; set; }
    public float PopulationGrowthRate { get; set; }
    public float AreaOccupiedByPark { get; set; }
    public string RelationshipStatus { get; set; } = string.Empty;
    public float AverageAnnualRainfall { get; set; }
    public string Landform { get; set; } = string.Empty;
    public string RainfallPattern { get; set; } = string.Empty;
    public string VegetationCharacteristics { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public List<string>? Tribes { get; set; } = new List<string>();
    public List<string>? LandHelds { get; set; } = new List<string>();
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public record DistrictProfileResponseDto : DistrictProfileDto
{
    public int RowNumber { get; set; }
}

public record DistrictContextTribeDto: CommunitySelectionDto
{
    public TribeDto? Tribe { get; set; } = new TribeDto();
}

public record GetDistrictProfileDto : DistrictProfileDto
{
    public List<DistrictContextTribeDto> Tribes { get; set; } = new List<DistrictContextTribeDto>();
    public List<CommunitySelectionDto> LandHelds { get; set; } = new List<CommunitySelectionDto>();
}

public record GetFullDistrictProfileDto : GetDistrictProfileDto
{
    public List<GovernmentLeaderDto> DistrictOfficials { get; set; } = new List<GovernmentLeaderDto>();
    public List<GovernmentLeaderDto> ParliamentMembers { get; set; } = new List<GovernmentLeaderDto>();
    public List<DistrictContextDto> GovernmentProgrammes { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> BenefitsFromPark { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> LanduseTrends { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> LanduseProblems { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> IssuesAffectingPark { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> NaturalResourceAreas { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> MarketAreas { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> BoundariesToPark { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> TourismFacilities { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> EconomicActivities { get; set; } = new List<DistrictContextDto>();
    public List<DistrictContextDto> BorderingVillages { get; set; } = new List<DistrictContextDto>();
    public List<DevelopmentOrganizationDto> DevelopmentOrganizations { get; set; } = new List<DevelopmentOrganizationDto>();
}