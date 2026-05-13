using conservation_backend.Features.LessLivestockDailies.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessLivestockDailies;

[Authorize]
[ApiController]
[Route("api/less-livestock-dailies")]
public class LessLivestockDailyController(ILessLivestockDailyService service) : ControllerBase
{
    private readonly ILessLivestockDailyService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithData<LessLivestockDailyResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] LessLivestockDailyGetRequest request)
    {
        var result = await _service.GetEntry(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithData<LessLivestockDailyResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Save([FromBody] LessLivestockDailySaveRequest request)
    {
        var result = await _service.SaveEntry(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("headers")]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessLivestockDailyHeaderDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHeaders([FromQuery] LessLivestockDailyHeadersRequest request)
    {
        var result = await _service.GetHeaders(request);
        return Ok(ApiHttpResponse.Page(result));
    }
}
