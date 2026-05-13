using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Shared;

namespace conservation_backend.Features.Vegetation
{
    public class Vegetation : BaseEntity
    {
        public Guid Id { get; set; }
        
        public Guid LocalNameId { get; set; }
        
        [ForeignKey("LocalNameId")] 
        public Location Location { get; set; } = null!;
        
        [MaxLength(20)]
        public string Session { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Rainfall { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Temperature { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Altitude { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Slope { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string SoilType { get; set; } = string.Empty;

        [MaxLength(250)]
        public string VegetationZone { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string VegetationType { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Coordinates { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Methodology { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? PlotId { get; set; } = string.Empty;
        
        [MaxLength(150)]
        public string? PlotSize { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? VegetationCategory { get; set; } = string.Empty;
        
        [MaxLength(250)]
        public string? StartCoordinate { get; set; } = string.Empty;
        
        [MaxLength(250)]
        public string? EndCoordinate { get; set; } = string.Empty;
        
        public string? Remark { get; set; } = string.Empty;
        
        public Guid? SpeciesId { get; set; }
        public Species.Species Species { get; set; } = null!;
        
        [MaxLength(100)]
        public string? OtherMethodology  { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? CommonNumber { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? SpeciesCount  { get; set; } = string.Empty;
        
        public Guid? ParkId { get; set; }
        public Park? Park { get; set; } = null;
    }
    
    public class LifeForm : BaseEntity
    {
        public Guid Id { get; set; }
        
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(150)]
        public string FamilyName { get; set; } = string.Empty;
        
        public Guid SpeciesId { get; set; }
        public Species.Species Species { get; set; } = null!;
        
        [MaxLength(50)]
        public string? Height { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? Weight { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? StemNumber { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? Diameter { get; set; } = string.Empty;
        
        [MaxLength(150)]
        public string? Cover { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? DiameterAtBreastHeight { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? Circumference { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? CanopyDiameter { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? CanopyClosure { get; set; } = string.Empty;
        
        public Guid VegetationId { get; set; }
        public Vegetation Vegetation { get; set; } = null!;
    }
    
    public class Disturbance : BaseEntity
    {
        public Guid Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Quantity { get; set; } = string.Empty;
        
        public Guid EntityId { get; set; }
        
        [MaxLength(50)]
        public string EntityName { get; set; } = string.Empty;
    }
    
    public class DistanceSample : BaseEntity
    {
        public Guid Id { get; set; }
        
        [MaxLength(250)]
        public string LongCoordinate { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string FamilyName { get; set; } = string.Empty;
        
        public Guid SpeciesId { get; set; }
        public Species.Species Species { get; set; } = null!;
        
        [MaxLength(100)]
        public string LeftDistance { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string RightDistance { get; set; } = string.Empty;
        
        public Guid EntityId { get; set; }
        
        [MaxLength(50)]
        public string EntityName { get; set; } = string.Empty;
    }
}
