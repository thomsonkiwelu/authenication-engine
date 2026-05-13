using authentication_engine.Features.SystemModules.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.SystemModules;

[Authorize]
[ApiController]
[Route("api/system-modules")]

public class SystemModuleController(ISystemModuleService systemModuleService): ControllerBase
{
    private readonly ISystemModuleService _systemModuleService = systemModuleService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<SystemModuleResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSystemModules(
        [FromQuery] SystemModulePaginationDto dto
    )
    {
        var pagedResult = await _systemModuleService.GetAllSystemModulesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<SystemModuleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SystemModuleRequestDto dto)
    {
        var result = await _systemModuleService.CreateSystemModule(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<SystemModuleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var systemModule = await _systemModuleService.GetSystemModuleById(id);

        return Ok(ApiHttpResponse.Data(systemModule));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<SystemModuleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SystemModuleRequestDto dto)
    {
        var result = await _systemModuleService.UpdateSystemModule(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _systemModuleService.DeleteSystemModule(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}