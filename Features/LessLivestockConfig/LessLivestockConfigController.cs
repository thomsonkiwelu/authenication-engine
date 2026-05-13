using conservation_backend.Features.LessLivestockConfig.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessLivestockConfig;

[Authorize]
[ApiController]
[Route("api/less-livestock-config")]
public class LessLivestockConfigController(ILessLivestockConfigService service) : ControllerBase
{
    private readonly ILessLivestockConfigService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithData<LessLivestockConfigResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfig()
    {
        var result = await _service.GetConfig();
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateConfig([FromBody] LessLivestockConfigUpdateRequest request)
    {
        await _service.UpdateConfig(request);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Updated));
    }
}
