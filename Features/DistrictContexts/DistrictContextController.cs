using conservation_backend.Features.DistrictContexts.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.DistrictContexts;

[Authorize]
[ApiController]
[Route("api/district-contexts")]

public class DistrictContextController(IDistrictContextService districtContextService) : ControllerBase
{
    private readonly IDistrictContextService _districtContextService = districtContextService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<DistrictContextResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] DistrictContextPaginationDto dto)
    {
        var pagedResult = await _districtContextService.GetAllDistrictContexts(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<DistrictContextDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DistrictContextRequestDto dto)
    {
        var result = await _districtContextService.CreateDistrictContext(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DistrictContextDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _districtContextService.GetDistrictContextById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DistrictContextDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] DistrictContextRequestDto dto)
    {
        var result = await _districtContextService.UpdateDistrictContext(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _districtContextService.DeleteDistrictContext(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
    [HttpPost("organizations")]
    [ProducesResponseType(typeof(ResponseWithData<DevelopmentOrganizationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrganization([FromBody] DevelopmentOrganizationRequestDto dto)
    {
        var result = await _districtContextService.CreateDevelopmentOrganization(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}/organizations")]
    [ProducesResponseType(typeof(ResponseWithData<DevelopmentOrganizationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateOrganization(Guid id, [FromBody] DevelopmentOrganizationRequestDto dto)
    {
        var result = await _districtContextService.UpdateDevelopmentOrganization(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}/organizations")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteOrganization(Guid id)
    {
        await _districtContextService.DeleteOrganization(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}