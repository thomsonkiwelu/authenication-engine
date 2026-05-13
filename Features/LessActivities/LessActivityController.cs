using conservation_backend.Features.LessActivities.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessActivities;

[Authorize]
[ApiController]
[Route("api/less-activities")]
public class LessActivityController(ILessActivityService lessActivityService) : ControllerBase
{
    private readonly ILessActivityService _lessActivityService = lessActivityService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessActivityResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] LessActivityPaginationDto dto)
    {
        var pagedResult = await _lessActivityService.GetAllLessActivitiesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<LessActivityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LessActivityRequest dto)
    {
        var result = await _lessActivityService.CreateLessActivity(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessActivityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _lessActivityService.GetLessActivityById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessActivityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LessActivityRequest dto)
    {
        var result = await _lessActivityService.UpdateLessActivity(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lessActivityService.DeleteLessActivity(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}