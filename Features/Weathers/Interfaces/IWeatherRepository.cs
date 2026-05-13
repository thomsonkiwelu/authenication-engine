using conservation_backend.Shared;

namespace conservation_backend.Features.Weathers.Interfaces;

public interface IWeatherRepository
{
    Task<PagedList<Weather>> GetPagedData(WeatherPaginationDto dto);

    Task<Weather> Create(Weather weather);

    Task<WeatherDto> GetById(Guid id);

    Task<string> Update(Guid id, WeatherRequest dto);

    Task<bool> Delete(Guid id);
}