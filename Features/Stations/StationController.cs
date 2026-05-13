using conservation_backend.Features.Stations.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Stations;

[Authorize]
[ApiController]
[Route("api/stations")]
public class StationController(IStationService stationService) : ControllerBase
{
    private readonly IStationService _stationService = stationService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<StationResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllLocations([FromQuery] StationPaginationDto dto)
    {
        var pagedResult = await _stationService.GetAllStationsData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<StationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStation([FromBody] StationRequest dto)
    {
        var result = await _stationService.CreateStation(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<StationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStationById(Guid id)
    {
        var result = await _stationService.GetStationById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<StationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStation(Guid id, [FromBody] StationRequest dto)
    {
        var result = await _stationService.UpdateStation(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteStation(Guid id)
    {
        await _stationService.DeleteStation(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}