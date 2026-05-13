using conservation_backend.Features.NestingTurtles.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.NestingTurtles;

[Authorize]
[ApiController]
[Route("api/sea-turtles/nestings")]

public class NestingTurtleController(INestingTurtleService nestingTurtleService) : ControllerBase
{
    private readonly INestingTurtleService _nestingTurtleService = nestingTurtleService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<NestingTurtleResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] NestingTurtlePaginationDto dto)
    {
        var pagedResult = await _nestingTurtleService.GetAllNestingTurtles(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<NestingTurtleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] NestingTurtleRequestDto dto)
    {
        var result = await _nestingTurtleService.CreateNestingTurtle(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<NestingTurtleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _nestingTurtleService.GetNestingTurtleById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<NestingTurtleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] NestingTurtleRequestDto dto)
    {
        var result = await _nestingTurtleService.UpdateNestingTurtle(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _nestingTurtleService.DeleteNestingTurtle(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}