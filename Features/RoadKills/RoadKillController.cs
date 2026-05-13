using conservation_backend.Features.RoadKills.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.RoadKills;

[Authorize]
[ApiController]
[Route("api/road-kills")]
public class RoadKillController(IRoadKillService roadKillService): Controller
{
    private readonly IRoadKillService _roadKillService = roadKillService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<RoadKillResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoadKills([FromQuery] RoadKillPaginationDto dto)
    {
        var pagedResult = await _roadKillService.GetPagedRoadKills(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetRoadKillDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoadKill([FromBody] RoadKillRequestDto dto)
    {
        var result = await _roadKillService.CreateRoadKill(dto);
            
        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetRoadKillDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoadKillById(Guid id)
    {
        var result = await _roadKillService.GetRoadKillById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetRoadKillDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRoadKill(Guid id, [FromBody] RoadKillRequestDto dto)
    {
        var result = await _roadKillService.UpdateRoadKill(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _roadKillService.DeleteRoadKill(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}