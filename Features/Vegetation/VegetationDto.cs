using conservation_backend.Features.Files;
using conservation_backend.Features.Locations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Species;
using conservation_backend.Features.Users;
using conservation_backend.Shared;
using conservation_backend.Shared.Dtos;

namespace conservation_backend.Features.Vegetation
{
    public record VegetationPaginationDto: PaginationDto
    {
        public string? VegetationType { get; init; } = string.Empty;

        public string? MethodologyType { get; set; } = string.Empty;
        
        public string? LifeFormType { get; set; } = string.Empty;
        
        public string? ParkId { get; set; } = string.Empty;
    }
    
    public record VegetationRequestDto
    {
        public Guid LocalNameId { get; set; }
        public string Session { get; set; } = string.Empty;
        public string Rainfall { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string Altitude { get; set; } = string.Empty;
        public string Slope { get; set; } = string.Empty;
        public string SoilType { get; set; } = string.Empty;
        public string VegetationZone { get; set; } = string.Empty;
        public string VegetationType { get; set; } = string.Empty;
        public string MethodologyType { get; set; } = string.Empty;
        public string? Coordinates { get; set; } = string.Empty;
        //Life form Inputs
        public string LifeFormType { get; set; } = string.Empty;
        public Guid LifeFormSpeciesId { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public string? DiameterAtBreastHeight { get; set; }
        public string? Circumference { get; set; }
        public string? Height { get; set; }
        public string? CanopyDiameter { get; set; }
        public string? CanopyClosure { get; set; }
        public string? StemNumber { get; set; }
        public string? Weight { get; set; }
        public string? Cover { get; set; }
        public string? Diameter { get; set; }
        //Plot methodology Inputs
        public string? PlotId { get; set; }
        public string? PlotSize { get; set; }
        //Distance Sample methodology Inputs
        public string? StartCoordinate { get; set; }
        public string? EndCoordinate { get; set; }
        public string? Remark { get; set; }
        public List<CoordinateAlongTransectRequestDto> CoordinateAlongTransects { get; set; } = new List<CoordinateAlongTransectRequestDto>();
        //Other method methodology Inputs
        public string? OtherSpeciesId { get; set; }
        public string? OtherMethodology { get; set; }
        public string? CommonNumber { get; set; }
        public string? SpeciesCount { get; set; }
        public string? VegetationCategory { get; set; }
        //Other Inputs
        public List<DisturbanceRequestDto> Disturbances { get; set; } = new List<DisturbanceRequestDto>();
        public List<FloraRequestDto> Floras { get; set; } = new List<FloraRequestDto>();
        public string? ImageId { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public record DisturbanceRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Quantity { get; set; } = string.Empty;
    }
    
    public record FloraRequestDto
    {
        public string ScientificNameId { get; set; } = string.Empty;
        public string? CommonName { get; set; } = string.Empty;
    }
    
    public record CoordinateAlongTransectRequestDto
    {
        public string LongCoordinate { get; set; } = string.Empty;
        public Guid SpeciesId { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public string RightDistance { get; set; } = string.Empty;
        public string LeftDistance { get; set; } = string.Empty;
    }
    
    public record VegetationDto
    {
        public Guid Id { get; set; }
        public string LocalNameId { get; set; } = string.Empty;
        public Guid ParkId { get; set; }
        public string Session { get; set; } = string.Empty;
        public string Rainfall { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string Altitude { get; set; } = string.Empty;
        public string Slope { get; set; } = string.Empty;
        public string SoilType { get; set; } = string.Empty;
        public string VegetationZone { get; set; } = string.Empty;
        public string VegetationType { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
        public string Methodology { get; set; } = string.Empty;
        public string PlotId { get; set; } = string.Empty;
        public string PlotSize { get; set; } = string.Empty;
        public string VegetationCategory { get; set; } = string.Empty;
        public string StartCoordinate { get; set; } = string.Empty;
        public string EndCoordinate { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string SpeciesId { get; set; } = string.Empty;
        public string OtherMethodology  { get; set; } = string.Empty;
        public string CommonNumber { get; set; } = string.Empty;
        public string SpeciesCount  { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        //Relationship
        public ParkDto Park { get; set; } = new ParkDto();
        public FileDto? File { get; set; } = new FileDto();
        public SpeciesDto Species { get; set; } = new SpeciesDto();
        public LocationDto Location { get; set; } = new LocationDto();
        public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
        public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
    }
    
    public record LifeFormDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string SpeciesId { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string StemNumber { get; set; } = string.Empty;
        public string Diameter { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string DiameterAtBreastHeight { get; set; } = string.Empty;
        public string Circumference { get; set; } = string.Empty;
        public string CanopyDiameter { get; set; } = string.Empty;
        public string CanopyClosure { get; set; } = string.Empty;
        public string VegetationId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        //Relationship
        public SpeciesDto Species { get; set; } = new SpeciesDto();
    }
    
    public record DisturbanceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    
    public record DistanceSampleDto
    {
        public Guid Id { get; set; }
        public string LongCoordinate { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string SpeciesId { get; set; }  = string.Empty;
        public string LeftDistance { get; set; } = string.Empty;
        public string RightDistance { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        //Relationship
        public SpeciesDto Species { get; set; } = new SpeciesDto();
    }
    
    public record VegetationResponseDto : VegetationDto
    {
        public int RowNumber { get; set; }
        public LifeFormDto LifeForm { get; set; } = new LifeFormDto();
        public LocationDto Location { get; set; } = new LocationDto();
        public SpeciesDto Species { get; set; } = new SpeciesDto();
    }
    
    public record ListVegetationSqlResponseDto
    {
        public List<VegetationResponseDto> Data { get; set; } = new();
        public PaginationMeta Meta { get; init; } = new ();
    }
    
    public record GetVegetationResponseDto : VegetationDto
    {
        public LifeFormDto LifeForm { get; set; } = new LifeFormDto();
        public List<DisturbanceDto> Disturbances { get; set; } = new List<DisturbanceDto>();
        public List<DistanceSampleDto> DistanceSamples { get; set; } = new List<DistanceSampleDto>();
        public List<SpeciesOccurrenceDto> SpeciesOccurrences { get; set; } = new List<SpeciesOccurrenceDto>();
        public LocationDto Location { get; set; } = new LocationDto();
    }

}
