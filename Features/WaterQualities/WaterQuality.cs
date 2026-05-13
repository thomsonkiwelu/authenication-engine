using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.WaterBodies;
using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQualities;

public class WaterQuality: BaseEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string WaterQualityType { get; set; } = string.Empty;
    
    public Guid WaterBodyId { get; set; }
    public WaterBody WaterBody { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
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
    
    [MaxLength(255)]    
    public string Coordinate { get; set; } =  string.Empty;
    
    public string Remark { get; set; } =  string.Empty;
}