using conservation_backend.Features.SystemModule.Interfaces;
using conservation_backend.Features.SystemModules.Interfaces;
using conservation_backend.Shared;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.SystemModules;

[Authorize]
[ApiController]
[Route("api/system-modules")]

public class SystemModuleController(ISystemModuleService systemModuleService): ControllerBase
{
    private readonly ISystemModuleService _systemModuleService = systemModuleService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<SystemModuleDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSystemModules(
        [FromQuery] PaginationDto dto
    )
    {
        var pagedResult = await _systemModuleService.GetAllSystemModulesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<SystemModuleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSystemModuleById(Guid id)
    {
        var systemModule = await _systemModuleService.GetSystemModuleById(id);

        return Ok(ApiHttpResponse.Data(systemModule));
    }
}