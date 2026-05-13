using conservation_backend.Features.LineTransects.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LineTransects;

[Authorize]
[ApiController]
[Route("api/line-transects")]

public class LineTransectController(ILineTransectService lineTransectService) : ControllerBase
{
    private readonly ILineTransectService _lineTransectService = lineTransectService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LineTransectResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] LineTransectPaginationDto dto)
    {
        var pagedResult = await _lineTransectService.GetPagedLineTransects(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetLineTransectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LineTransectRequestDto dto)
    {
        var result = await _lineTransectService.CreateLineTransect(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetLineTransectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _lineTransectService.GetLineTransectById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetLineTransectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LineTransectRequestDto dto)
    {
        var result = await _lineTransectService.UpdateLineTransect(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lineTransectService.DeleteLineTransect(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}