using authentication_engine.Features.LessRangerStations.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.LessRangerStations;

[Authorize]
[ApiController]
[Route("api/less-ranger-stations")]
public class LessRangerStationController(ILessRangerStationService service) : ControllerBase
{
    private readonly ILessRangerStationService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessRangerStationResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetStations([FromQuery] LessRangerStationPaginationDto dto)
    {
        var pagedResult = await _service.GetAllStationsData(dto);
        return Ok(ApiHttpResponse.Page(pagedResult));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerStationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStation([FromBody] LessRangerStationRequest dto)
    {
        var result = await _service.CreateStation(dto);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerStationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStationById(Guid id)
    {
        var result = await _service.GetStationById(id);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerStationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStation(Guid id, [FromBody] LessRangerStationRequest dto)
    {
        var result = await _service.UpdateStation(id, dto);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteStation(Guid id)
    {
        await _service.DeleteStation(id);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}
