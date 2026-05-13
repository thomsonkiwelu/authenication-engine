using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.WaterBodies;
using conservation_backend.Shared;

namespace conservation_backend.Features.WaterQuantities;

public class WaterQuantity: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid WaterBodyId { get; set; }
    public WaterBody WaterBody { get; set; } = null!;
    
    public Guid ParkId { get; set; }
    public Park Park { get; set; } = null!;
    
    public float WaterLevel { get; set; }
    
    public float WettedPerimeter { get; set; }
    
    public float WettedWidth { get; set; }
    
    public float AverageDepth { get; set; }
    
    public float Length { get; set; }
    
    public float AverageTime { get; set; }
    
    public float MinimumFlowRate { get; set; }
    
    public float MaximumFlowRate { get; set; }
    
    public float AverageFlowRate { get; set; }
    
    public float CalculatedDischargeRate { get; set; }
    
    public float Volume { get; set; }
    
    [MaxLength(255)]    
    public string Coordinate { get; set; } =  string.Empty;
    
    public string Remark { get; set; } =  string.Empty;
}