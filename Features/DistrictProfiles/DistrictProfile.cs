using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Districts;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.DistrictProfiles;

public class DistrictProfile : BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid DistrictId { get; set; }
    public District District { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    public float AreaSize { get; set; }
    
    public float Population { get; set; }
    
    public float PopulationGrowthRate { get; set; }
    
    public float AreaOccupiedByPark { get; set; }
    
    [MaxLength(50)]
    public string RelationshipStatus { get; set; } = string.Empty;
    
    public float AverageAnnualRainfall { get; set; }
    
    [MaxLength(250)]
    public string Landform { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string RainfallPattern { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string VegetationCharacteristics { get; set; } = string.Empty;
    
}