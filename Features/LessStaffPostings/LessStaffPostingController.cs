using conservation_backend.Features.LessStaffPostings.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.LessStaffPostings;

[Authorize]
[ApiController]
[Route("api/less-staff-postings")]
public class LessStaffPostingController(ILessStaffPostingService service) : ControllerBase
{
    private readonly ILessStaffPostingService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<LessStaffPostingDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPostings([FromQuery] LessStaffPostingPaginationDto dto)
    {
        var result = await _service.GetPostings(dto);
        return Ok(ApiHttpResponse.Page(result));
    }

    [HttpPost("assign")]
    [ProducesResponseType(typeof(ResponseWithData<LessStaffPostingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Assign([FromBody] LessStaffPostingAssignRequest request)
    {
        var result = await _service.Assign(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPost("assign/bulk")]
    [ProducesResponseType(typeof(ResponseWithData<List<LessStaffPostingDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BulkAssign([FromBody] LessStaffPostingBulkAssignRequest request)
    {
        var result = await _service.BulkAssign(request);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpPost("unassign")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Unassign([FromBody] LessStaffPostingUnassignRequest request)
    {
        await _service.Unassign(request);
        return Ok(ApiHttpResponse.Message(ResponseMessages.Updated));
    }

    [HttpGet("park/{parkId}/staff-options")]
    [ProducesResponseType(typeof(ResponseWithData<List<StaffOptionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetParkStaffOptions(string parkId)
    {
        var result = await _service.GetParkStaffOptions(parkId);
        return Ok(ApiHttpResponse.Data(result));
    }

    [HttpGet("office/{officeId}/staff-options")]
    [ProducesResponseType(typeof(ResponseWithData<List<StaffOptionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOfficeStaffOptions(string officeId)
    {
        var result = await _service.GetOfficeStaffOptions(officeId);
        return Ok(ApiHttpResponse.Data(result));
    }
}
