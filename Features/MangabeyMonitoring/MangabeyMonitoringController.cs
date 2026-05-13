using conservation_backend.Features.MangabeyMonitoring.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.MangabeyMonitoring;

[Authorize]
[ApiController]
[Route("api/mangabey-monitoring")]
public class MangabeyMonitoringController(IMangabeyMonitoringService mangabeyMonitoringService) : ControllerBase
{
    private readonly IMangabeyMonitoringService _mangabeyMonitoringService = mangabeyMonitoringService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<MangabeyMonitoringResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] MangabeyMonitoringPaginationDto dto)
    {
        var pagedResult = await _mangabeyMonitoringService.GetAllMangabeyMonitoringData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<MangabeyMonitoringDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] MangabeyMonitoringRequestDto dto)
    {
        var result = await _mangabeyMonitoringService.CreateMangabeyMonitoring(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<MangabeyMonitoringDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mangabeyMonitoringService.GetMangabeyMonitoringById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<MangabeyMonitoringDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] MangabeyMonitoringRequestDto dto)
    {
        var result = await _mangabeyMonitoringService.UpdateMangabeyMonitoring(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mangabeyMonitoringService.DeleteMangabeyMonitoring(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}