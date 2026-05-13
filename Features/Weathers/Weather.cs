using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Stations;
using conservation_backend.Shared;

namespace conservation_backend.Features.Weathers;

public class Weather: BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid StationId { get; set; }
    public Station Station { get; set; } = null!;
    
    [MaxLength(200)]
    public string TmaRegistrationNumber { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string CollectionMethod { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string? RainfallFrequency { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Rainfall { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string MinimumTemperature { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string MaximumTemperature { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string MeanTemperature { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string WindDirection { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string WindSpeed { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string AverageWindSpeed { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string DryHumidity { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string WetHumidity { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string AverageHumidity { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string CloudCover { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Sunshine { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Pressure { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Evaporation { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string Radiation { get; set; } = string.Empty;
    
    [MaxLength(150)]
    public string DeviceName { get; set; } = string.Empty;
    
    [MaxLength(250)]
    public string? Coordinates { get; set; } = string.Empty;
    
    public Guid? ParkId { get; set; }
    public Park? Park { get; set; } = null;
}