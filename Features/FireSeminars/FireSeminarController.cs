using conservation_backend.Features.FireSeminars.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.FireSeminars;

[Authorize]
[ApiController]
[Route("api/fire-seminars")]
public class FireSeminarController(IFireSeminarService fireSeminarService): ControllerBase
{
    private readonly IFireSeminarService _fireSeminarService = fireSeminarService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<FireSeminarResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] FireSeminarPaginationDto dto)
    {
        var pagedResult = await _fireSeminarService.GetFireSeminars(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetFireSeminarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FireSeminarRequestDto dto)
    {
        var result = await _fireSeminarService.CreateFireSeminar(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetFireSeminarDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _fireSeminarService.GetFireSeminarById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetFireSeminarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] FireSeminarRequestDto dto)
    {
        var result = await _fireSeminarService.UpdateFireSeminar(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _fireSeminarService.DeleteFireSeminar(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}