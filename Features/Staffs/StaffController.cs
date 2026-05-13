using authentication_engine.Features.Staffs.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.Staffs;

[Authorize]
[ApiController]
[Route("api/staffs")]
public class StaffController(IStaffService staffService) : ControllerBase
{   
    private readonly IStaffService _staffService = staffService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<StaffResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllStaffs([FromQuery] StaffPaginationDto dto)
    {
        var pagedResult = await _staffService.GetAllStaffsData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<StaffDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateStaff([FromBody] StaffRequest dto)
    {
        var result = await _staffService.CreateStaff(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<StaffDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStaffById(Guid id)
    {
        var result = await _staffService.GetStaffById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<StaffDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] StaffRequest dto)
    {
        var result = await _staffService.UpdateStaff(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteStaff(Guid id)
    {
        await _staffService.DeleteStaff(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
    [HttpGet("organization-context/{officeId}")]
    [ProducesResponseType(typeof(ResponseWithData<OrganizationContextDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetStaffsWithOrganizationContext(Guid officeId)
    {
        var result = await _staffService.GetStaffsWithOrganizationContext(officeId);
            
        return Ok(ApiHttpResponse.Data(result));
    }
    
}