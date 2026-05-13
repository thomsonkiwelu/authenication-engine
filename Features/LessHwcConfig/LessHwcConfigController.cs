using conservation_backend.Features.LessHwcConfig.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessHwcConfig;

[Authorize]
[ApiController]
[Route("api/less-hwc-config")]
public class LessHwcConfigController(ILessHwcConfigService service) : ControllerBase
{
    private readonly ILessHwcConfigService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithData<LessHwcConfigResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfig()
    {
        var result = await _service.GetConfig();
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateConfig([FromBody] LessHwcConfigUpdateRequest request)
    {
        await _service.UpdateConfig(request);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Updated));
    }
}
