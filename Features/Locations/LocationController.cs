using conservation_backend.Features.Locations.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Locations;

[Authorize]
[ApiController]
[Route("api/locations")]
public class LocationController(ILocationService locationService) : ControllerBase
{
    private readonly ILocationService _locationService = locationService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LocationResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllLocations([FromQuery] LocationPaginationDto dto)
    {
        var pagedResult = await _locationService.GetAllLocationsData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<LocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocation([FromBody] LocationRequest dto)
    {
        var result = await _locationService.CreateLocation(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocationById(Guid id)
    {
        var result = await _locationService.GetLocationById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] LocationRequest dto)
    {
        var result = await _locationService.UpdateLocation(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteLocation(Guid id)
    {
        await _locationService.DeleteLocation(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}