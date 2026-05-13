using conservation_backend.Features.LessPatrols.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessPatrols;

[Authorize]
[ApiController]
[Route("api/less-patrol-dailies")]
public class LessPatrolDailyController(ILessPatrolDailyService service) : ControllerBase
{
    private readonly ILessPatrolDailyService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithData<LessPatrolDailyResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] LessPatrolDailyGetRequest request)
    {
        var result = await _service.GetEntry(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithData<LessPatrolDailyResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Save([FromBody] LessPatrolDailySaveRequest request)
    {
        var result = await _service.SaveEntry(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("headers")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessPatrolDailyHeaderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHeaders([FromQuery] LessPatrolDailyHeadersRequest request)
    {
        var result = await _service.GetHeaders(request);
        return Ok(ApiHttpResponse.Page(result));
    }
}
