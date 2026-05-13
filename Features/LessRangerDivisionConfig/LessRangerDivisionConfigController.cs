using conservation_backend.Features.LessRangerDivisionConfig.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessRangerDivisionConfig;

[Authorize]
[ApiController]
[Route("api/less-ranger-division-config")]
public class LessRangerDivisionConfigController(ILessRangerDivisionConfigService service) : ControllerBase
{
    private readonly ILessRangerDivisionConfigService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerDivisionConfigResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfig()
    {
        var result = await _service.GetConfig();
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateConfig([FromBody] LessRangerDivisionConfigUpdateRequest request)
    {
        await _service.UpdateConfig(request);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Updated));
    }
}
