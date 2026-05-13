using conservation_backend.Features.DistrictProfiles.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.DistrictProfiles;

[Authorize]
[ApiController]
[Route("api/district-profiles")]
public class DistrictProfileController(IDistrictProfileService districtProfileService) : ControllerBase
{
    private readonly IDistrictProfileService _districtProfileService = districtProfileService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<DistrictProfileResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] DistrictProfilePaginationDto dto)
    {
        var pagedResult = await _districtProfileService.GetDistrictProfiles(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<DistrictProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DistrictProfileRequestDto dto)
    {
        var result = await _districtProfileService.CreateDistrictProfile(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DistrictProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _districtProfileService.GetDistrictProfileById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DistrictProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] DistrictProfileRequestDto dto)
    {
        var result = await _districtProfileService.UpdateDistrictProfile(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _districtProfileService.DeleteDistrictProfile(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
    [HttpGet("{id}/details")]
    [ProducesResponseType(typeof(ResponseWithData<GetFullDistrictProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFullDistrictProfileById(Guid id)
    {
        var result = await _districtProfileService.GetFullDistrictProfileById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
}