using conservation_backend.Features.Districts.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Districts;

[Authorize]
[ApiController]
[Route("api/districts")]
public class DistrictController(IDistrictService districtService) : ControllerBase
{
    private readonly IDistrictService _districtService = districtService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<DistrictResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] DistrictPaginationDto dto)
    {
        var pagedResult = await _districtService.GetAllDistricts(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<DistrictDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DistrictRequest dto)
    {
        var result = await _districtService.CreateDistrict(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DistrictDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _districtService.GetDistrictById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DistrictDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] DistrictRequest dto)
    {
        var result = await _districtService.UpdateDistrict(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _districtService.DeleteDistrict(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}