using conservation_backend.Features.FireBreaks.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.FireBreaks;

[Authorize]
[ApiController]
[Route("api/firebreaks")]

public class FireBreakController(IFireBreakService fireBreakService): ControllerBase
{
    private readonly IFireBreakService _fireBreakService = fireBreakService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<FireBreakResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] FireBreakPaginationDto dto)
    {
        var pagedResult = await _fireBreakService.GetFireBreaks(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetFireBreakDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FireBreakRequestDto dto)
    {
        var result = await _fireBreakService.CreateFireBreak(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetFireBreakDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _fireBreakService.GetFireBreakById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetFireBreakDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] FireBreakRequestDto dto)
    {
        var result = await _fireBreakService.UpdateFireBreak(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _fireBreakService.DeleteFireBreak(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}