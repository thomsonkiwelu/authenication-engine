using conservation_backend.Features.LessHwcIncidents.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessHwcIncidents;

[Authorize]
[ApiController]
[Route("api/less-hwc-incidents")]
public class LessHwcIncidentController(ILessHwcIncidentService service) : ControllerBase
{
    private readonly ILessHwcIncidentService _service = service;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseWithData<LessHwcIncidentResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _service.GetById(id);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<LessHwcIncidentResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] LessHwcIncidentCreateRequest request)
    {
        var result = await _service.Create(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseWithData<LessHwcIncidentResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] LessHwcIncidentUpdateRequest request)
    {
        var result = await _service.Update(id, request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _service.Delete(id);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }

    [HttpGet("headers")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessHwcIncidentHeaderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHeaders([FromQuery] LessHwcIncidentHeadersRequest request)
    {
        var result = await _service.GetHeaders(request);
        return Ok(ApiHttpResponse.Page(result));
    }
}
