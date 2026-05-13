using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Stations;
using conservation_backend.Shared;

namespace conservation_backend.Features.Wastes
{
    public class Waste : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid StationId { get; set; }
        public Station Station { get; set; } = null!;
        
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;
        
        [MaxLength(250)]
        public string? Coordinates { get; set; } = string.Empty;
        
        [MaxLength(250)]
        public string? SolidStateRemark { get; set; } = string.Empty;
        
        [MaxLength(250)]
        public string? LiquidStateRemark { get; set; } = string.Empty;
        
        public Guid? ParkId { get; set; }
        public Park? Park { get; set; } = null;
    }
    
    public class WasteMaterial : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid WasteId { get; set; }
        public Waste Waste { get; set; } = null!;
        
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? OtherName { get; set; } = string.Empty;
        
        public float Quantity { get; set; }
    }
}
