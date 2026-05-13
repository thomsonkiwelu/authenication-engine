using conservation_backend.Features.Tribes.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Tribes;

[Authorize]
[ApiController]
[Route("api/tribes")]
public class TribeController(ITribeService tribeService) : ControllerBase
{
    private readonly ITribeService _tribeService = tribeService;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<TribeDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTribes([FromQuery] PaginationDto dto)
    {
        var pagedResult = await _tribeService.GetAllTribesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<TribeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTribe([FromBody] TribeRequestDto dto)
    {
        var result = await _tribeService.CreateTribe(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<TribeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTribeById(Guid id)
    {
        var result = await _tribeService.GetTribeById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<TribeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTribe(Guid id, [FromBody] TribeRequestDto dto)
    {
        var result = await _tribeService.UpdateTribe(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTribe(Guid id)
    {
        await _tribeService.DeleteTribe(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}