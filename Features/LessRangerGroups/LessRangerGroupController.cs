using authentication_engine.Features.LessRangerGroups.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.LessRangerGroups;

[Authorize]
[ApiController]
[Route("api/less-ranger-groups")]
public class LessRangerGroupController(ILessRangerGroupService service) : ControllerBase
{
    private readonly ILessRangerGroupService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessRangerGroupResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetGroups([FromQuery] LessRangerGroupPaginationDto dto)
    {
        var pagedResult = await _service.GetAllGroupsData(dto);
        return Ok(ApiHttpResponse.Page(pagedResult));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroup([FromBody] LessRangerGroupRequest dto)
    {
        var result = await _service.CreateGroup(dto);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerGroupDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupById(Guid id)
    {
        var result = await _service.GetGroupById(id);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<LessRangerGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] LessRangerGroupRequest dto)
    {
        var result = await _service.UpdateGroup(id, dto);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        await _service.DeleteGroup(id);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}
