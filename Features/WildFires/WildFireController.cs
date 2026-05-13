using conservation_backend.Features.WildFires.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.WildFires.WildFireController;

[Authorize]
[ApiController]
[Route("api/wildfires")]

public class WildFireController(IWildFireService wildFireService): ControllerBase
{
    private readonly IWildFireService _wildFireService = wildFireService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<WildFireResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] WildFirePaginationDto dto)
    {
        var pagedResult = await _wildFireService.GetWildFires(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetWildFireDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] WildFireRequestDto dto)
    {
        var result = await _wildFireService.CreateWildFire(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetWildFireDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _wildFireService.GetWildFireById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetWildFireDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] WildFireRequestDto dto)
    {
        var result = await _wildFireService.UpdateWildFire(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _wildFireService.DeleteWildFire(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}