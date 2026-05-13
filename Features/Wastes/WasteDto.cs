using conservation_backend.Features.Parks;
using conservation_backend.Features.Stations;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Wastes
{

    public record WastePaginationDto : PaginationDto
    {
        public string? Category { get; set; }
        public string? ParkId { get; set; }
    }
    
    public record WasteDto
    {
        public Guid Id { get; set; }
        public Guid StationId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
        public string SolidStateRemark { get; set; } = string.Empty;
        public string LiquidStateRemark { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public float TotalSolidState { get; set; }
        public float TotalLiquidState { get; set; }
        //Relationship
        public ParkDto Park { get; set; } = new ParkDto();
        public StationDto Station { get; set; } = new StationDto();
        public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
        public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
    }
    
    public record WasteMaterialDto
    {
        public Guid Id { get; set; }
        public Guid WasteId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string OtherName { get; set; } = string.Empty;
        public float Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
    
    public record WasteResponseDto : WasteDto
    {
        public int RowNumber { get; set; }
    }
    
    public record GetWasteDto : WasteDto
    {
        public List<WasteMaterialDto> WasteInSolidState { get; set; } = new List<WasteMaterialDto>();
        public List<WasteMaterialDto> WasteInLiquidState { get; set; } = new List<WasteMaterialDto>();
    }

    public record WasteSqlResponseDto
    {
        public List<WasteResponseDto> Data { get; set; } = new();
    
        public PaginationMeta Meta { get; init; } = new ();
    }
    
    public record WasteRequestDto
    {
        public Guid StationId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string SolidStateRemark { get; set; } = string.Empty;
        public string LiquidStateRemark { get; set; } = string.Empty;
        public string? Coordinates { get; set; } = string.Empty;
        public List<WasteStateItemDto> SolidStates { get; set; } = new List<WasteStateItemDto>();
        public List<WasteStateItemDto> LiquidStates { get; set; } = new List<WasteStateItemDto>();
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public record WasteStateItemDto
    {
        public string Name { get; set; } = string.Empty;
        public float Quantity { get; set; }
        public string? OtherName { get; set; } = string.Empty;
    }
    
}
