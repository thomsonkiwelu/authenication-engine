using conservation_backend.Shared;

namespace conservation_backend.Features.Weathers.Interfaces;

public interface IWeatherService
{
    Task<PagedList<WeatherResponseDto>> GetPagedWeather(WeatherPaginationDto dto);
    
    Task<WeatherDto> CreateWeather(WeatherRequest dto);
    
    Task<WeatherDto> GetWeatherById(Guid id);

    Task<WeatherDto> UpdateWeather(Guid id , WeatherRequest dto);

    Task<bool> DeleteWeather(Guid id);
}