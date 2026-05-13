using conservation_backend.Features.GroundCounts.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.GroundCounts;

[Authorize]
[ApiController]
[Route("api/ground-counts")]
public class GroundCountController(IGroundCountService groundCountService) : ControllerBase
{
    private readonly IGroundCountService _groundCountService = groundCountService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<GroundCountResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] GroundCountPaginationDto dto)
    {
        var pagedResult = await _groundCountService.GetGroundCounts(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetGroundCountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] GroundCountRequestDto dto)
    {
        var result = await _groundCountService.CreateGroundCount(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetGroundCountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _groundCountService.GetGroundCountById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetGroundCountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] GroundCountRequestDto dto)
    {
        var result = await _groundCountService.UpdateGroundCount(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _groundCountService.DeleteGroundCount(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}