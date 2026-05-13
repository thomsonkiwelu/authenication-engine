using conservation_backend.Features.Weathers.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Weathers;

[Authorize]
[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weatherService): Controller
{
    private readonly IWeatherService _weatherService = weatherService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<WeatherResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeathers([FromQuery] WeatherPaginationDto dto)
    {
        var pagedResult = await _weatherService.GetPagedWeather(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<WeatherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWeather([FromBody] WeatherRequest dto)
    {
        var result = await _weatherService.CreateWeather(dto);
            
        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WeatherDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeatherById(Guid id)
    {
        var result = await _weatherService.GetWeatherById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WeatherDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateWeather(Guid id, [FromBody] WeatherRequest dto)
    {
        var result = await _weatherService.UpdateWeather(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteWeather(Guid id)
    {
        await _weatherService.DeleteWeather(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}