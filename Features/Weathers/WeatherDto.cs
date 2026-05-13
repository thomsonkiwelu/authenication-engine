using conservation_backend.Features.Files;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Stations;
using conservation_backend.Features.Users;
using conservation_backend.Shared;

namespace conservation_backend.Features.Weathers;

public record WeatherPaginationDto: PaginationDto
{
    public string? ParkId { get; set; } = string.Empty;
    public string? Method { get; set; } = string.Empty;
}

public record WeatherRequest
{
    public Guid StationId { get; set; }
    public string TmaRegistrationNumber { get; set; } = string.Empty;
    public string CollectionMethod { get; set; } = string.Empty;
    public string RainfallFrequency { get; set; } = string.Empty;
    public string Rainfall { get; set; } = string.Empty;
    public string MinimumTemperature { get; set; } = string.Empty;
    public string MaximumTemperature { get; set; } = string.Empty;
    public string MeanTemperature { get; set; } = string.Empty;
    public string WindDirection { get; set; } = string.Empty;
    public string WindSpeed { get; set; } = string.Empty;
    public string AverageWindSpeed { get; set; } = string.Empty;
    public string DryHumidity { get; set; } = string.Empty;
    public string WetHumidity { get; set; } = string.Empty;
    public string AverageHumidity { get; set; } = string.Empty;
    public string CloudCover { get; set; } = string.Empty;
    public string Sunshine { get; set; } = string.Empty;
    public string Pressure { get; set; } = string.Empty;
    public string Evaporation { get; set; } = string.Empty;
    public string Radiation { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public string? AttachmentId { get; set; } = string.Empty;
    public Guid? UpdatedBy { get; set; }
}

public record WeatherDto
{
    public Guid Id { get; set; }
    public Guid StationId { get; set; }
    public Guid ParkId { get; set; }
    public string TmaRegistrationNumber { get; set; } = string.Empty;
    public string CollectionMethod { get; set; } = string.Empty;
    public string RainfallFrequency { get; set; } = string.Empty;
    public string Rainfall { get; set; } = string.Empty;
    public string MinimumTemperature { get; set; } = string.Empty;
    public string MaximumTemperature { get; set; } = string.Empty;
    public string MeanTemperature { get; set; } = string.Empty;
    public string WindDirection { get; set; } = string.Empty;
    public string WindSpeed { get; set; } = string.Empty;
    public string AverageWindSpeed { get; set; } = string.Empty;
    public string DryHumidity { get; set; } = string.Empty;
    public string WetHumidity { get; set; } = string.Empty;
    public string AverageHumidity { get; set; } = string.Empty;
    public string CloudCover { get; set; } = string.Empty;
    public string Sunshine { get; set; } = string.Empty;
    public string Pressure { get; set; } = string.Empty;
    public string Evaporation { get; set; } = string.Empty;
    public string Radiation { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Coordinates { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    //Relationship
    public ParkDto Park { get; set; } = new ParkDto();
    public FileDto? File { get; set; } = new FileDto();
    public StationDto Station { get; set; } = new StationDto();
    public UserMinimalDto Creator { get; set; } = new UserMinimalDto();
    public UserMinimalDto CreatedByUser { get; set; } = new UserMinimalDto();
    public UserMinimalDto UpdatedByUser { get; set; } = new UserMinimalDto();
}

public record WeatherResponseDto : WeatherDto
{
    public int RowNumber { get; set; }
}

