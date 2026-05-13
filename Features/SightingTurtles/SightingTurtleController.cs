using conservation_backend.Features.SightingTurtles.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.SightingTurtles;

[Authorize]
[ApiController]
[Route("api/sea-turtles/sightings")]

public class SightingTurtleController(ISightingTurtleService sightingTurtleService): ControllerBase
{
    private readonly ISightingTurtleService _sightingTurtleService = sightingTurtleService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<SightingTurtleResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] SightingTurtlePaginationDto dto)
    {
        var pagedResult = await _sightingTurtleService.GetSightingTurtles(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetSightingTurtleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SightingTurtleRequestDto dto)
    {
        var result = await _sightingTurtleService.CreateSightingTurtle(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetSightingTurtleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sightingTurtleService.GetSightingTurtleById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetSightingTurtleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SightingTurtleRequestDto dto)
    {
        var result = await _sightingTurtleService.UpdateSightingTurtle(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sightingTurtleService.DeleteSightingTurtle(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}