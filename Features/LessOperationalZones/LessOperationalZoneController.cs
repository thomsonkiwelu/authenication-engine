using conservation_backend.Features.LessOperationalZones.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessOperationalZones;

[Authorize]
[ApiController]
[Route("api/less-operational-zones")]
public class LessOperationalZoneController(ILessOperationalZoneService service) : ControllerBase
{
    private readonly ILessOperationalZoneService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessOperationalZoneResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetZones([FromQuery] LessOperationalZonePaginationDto dto)
    {
        var pagedResult = await _service.GetAllZonesData(dto);
        return Ok(ApiHttpResponse.Page(pagedResult));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<LessOperationalZoneDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateZone([FromBody] LessOperationalZoneRequest dto)
    {
        var result = await _service.CreateZone(dto);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessOperationalZoneDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetZoneById(Guid id)
    {
        var result = await _service.GetZoneById(id);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessOperationalZoneDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateZone(Guid id, [FromBody] LessOperationalZoneRequest dto)
    {
        var result = await _service.UpdateZone(id, dto);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteZone(Guid id)
    {
        await _service.DeleteZone(id);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}
