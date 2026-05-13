using conservation_backend.Features.Files;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Users;
using conservation_backend.Features.WaterBodies;
using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQualities.Interfaces;

public record WaterQualityPaginationDto : PaginationDto
{
    public string? ParkId { get; set; }
    
    public string? WaterBodyId { get; set; }
    
    public string? WaterBodyType { get; set; }
}

public record WaterQualityDto
{
    public Guid Id { get; set; }
    public string WaterQualityType { get; set; } = string.Empty;
    public Guid WaterBodyId { get; set; }
    public Guid ParkId { get; set; }
    public float Temperature { get; set; }
    public float AtmosphericPressure { get; set; }
    public float OxidationReductionPotential { get; set; }
    public float DissolvedOxygenInPercentage { get; set; }
    public float DissolvedOxygenInMg { get; set; }
    public float TotalDissolvedSolid { get; set; }
    public float Resistivity { get; set; }
    public float SalinityInPpt { get; set; }
    public float SalinityInPercentage { get; set; }
    public float Ssg { get; set; }
    public float WaterFlowRate { get; set; }
    public float FecalColiform { get; set; }
    public float TotalColiform { get; set; }
    public float PotentialOfHydrogen { get; set; }
    public float ElectricConductivity { get; set; }
    public float Nitrate { get; set; }
    public float Fluoride { get; set; }
    public float Chloride { get; set; }
    public float TotalAlkalinity { get; set; }
    public float Phosphate { get; set; }
    public float Turbidity { get; set; }
    public float Color { get; set; }
    public float Settleable { get; set; }
    public float TotalHardness { get; set; }
    public float Calcium { get; set; }
    public float Magnesium { get; set; }
    public float Iron { get; set; }
    public float Copper { get; set; }
    public float Chromium { get; set; }
    public float Ammonia { get; set; }
    public float Nitrite { get; set; }
    public float Sulphate { get; set; }
    public float Sodium { get; set; }
    public float Potassium { get; set; }
    public float TotalSuspendedSolid { get; set; }
    public string? Coordinate { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public FileDto? File { get; set; } = new FileDto();
    public WaterBodyDto WaterBody { get; set; } = new WaterBodyDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto Updater { get; set; } = new UserMinimalDto();
}

public record  WaterQualityResponseDto
{
    public Guid Id { get; set; }
    public int RowNumber { get; set; }
    public string WaterQualityType { get; set; } = string.Empty;
    public Guid WaterBodyId { get; set; }
    public Guid ParkId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public ParkDto Park { get; set; } = new ParkDto();
    public WaterBodyDto WaterBody { get; set; } = new WaterBodyDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
}

public record WaterQualityRequestDto
{
    public Guid WaterBodyId { get; set; }
    public string WaterQualityType { get; set; } = string.Empty;
    public float Temperature { get; set; }
    public float AtmosphericPressure { get; set; }
    public float OxidationReductionPotential { get; set; }
    public float DissolvedOxygenInPercentage { get; set; }
    public float DissolvedOxygenInMg { get; set; }
    public float TotalDissolvedSolid { get; set; }
    public float Resistivity { get; set; }
    public float SalinityInPpt { get; set; }
    public float SalinityInPercentage { get; set; }
    public float Ssg { get; set; }
    public float WaterFlowRate { get; set; }
    public float FecalColiform { get; set; }
    public float TotalColiform { get; set; }
    public float PotentialOfHydrogen { get; set; }
    public float ElectricConductivity { get; set; }
    public float Nitrate { get; set; }
    public float Fluoride { get; set; }
    public float Chloride { get; set; }
    public float TotalAlkalinity { get; set; }
    public float Phosphate { get; set; }
    public float Turbidity { get; set; }
    public float Color { get; set; }
    public float Settleable { get; set; }
    public float TotalHardness { get; set; }
    public float Calcium { get; set; }
    public float Magnesium { get; set; }
    public float Iron { get; set; }
    public float Copper { get; set; }
    public float Chromium { get; set; }
    public float Ammonia { get; set; }
    public float Nitrite { get; set; }
    public float Sulphate { get; set; }
    public float Sodium { get; set; }
    public float Potassium { get; set; }
    public float TotalSuspendedSolid { get; set; }
    public string? Coordinate { get; set; } = string.Empty;
    public string? ImageId { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
}