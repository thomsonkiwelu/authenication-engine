using conservation_backend.Features.GovernmentLeaders.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.GovernmentLeaders;

[Authorize]
[ApiController]
[Route("api/government-leaders")]
public class GovernmentLeaderController(IGovernmentLeaderService governmentLeaderService) : ControllerBase
{
    private readonly IGovernmentLeaderService _governmentLeaderService = governmentLeaderService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<GovernmentLeaderResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] GovernmentLeaderPaginationDto dto)
    {
        var pagedResult = await _governmentLeaderService.GetAllGovernmentLeaders(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GovernmentLeaderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] GovernmentLeaderRequestDto dto)
    {
        var result = await _governmentLeaderService.CreateGovernmentLeader(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GovernmentLeaderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _governmentLeaderService.GetGovernmentLeaderById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GovernmentLeaderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] GovernmentLeaderRequestDto dto)
    {
        var result = await _governmentLeaderService.UpdateGovernmentLeader(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _governmentLeaderService.DeleteGovernmentLeader(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}