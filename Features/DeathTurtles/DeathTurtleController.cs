using conservation_backend.Features.DeathTurtles.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.DeathTurtles;

[Authorize]
[ApiController]
[Route("api/sea-turtles/deaths")]

public class DeathTurtleController(IDeathTurtleService deathTurtleService) : ControllerBase
{
    private readonly IDeathTurtleService _deathTurtleService = deathTurtleService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<DeathTurtleResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] DeathTurtlePaginationDto dto)
    {
        var pagedResult = await _deathTurtleService.GetAllDeathTurtles(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<DeathTurtleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DeathTurtleRequestDto dto)
    {
        var result = await _deathTurtleService.CreateDeathTurtle(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DeathTurtleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _deathTurtleService.GetDeathTurtleById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DeathTurtleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] DeathTurtleRequestDto dto)
    {
        var result = await _deathTurtleService.UpdateDeathTurtle(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deathTurtleService.DeleteDeathTurtle(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}