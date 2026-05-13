using conservation_backend.Features.WaterQualities.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.WaterQualities;

[Authorize]
[ApiController]
[Route("api/water-qualities")]

public class WaterQualityController(IWaterQualityService waterQualityService): ControllerBase
{
    private readonly IWaterQualityService _waterQualityService = waterQualityService;
        
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<WaterQualityRequestDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllWaterQualities([FromQuery] WaterQualityPaginationDto dto)
    {
        var pagedResult = await _waterQualityService.GetAllWaterQualitiesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<WaterQualityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWaterQuality([FromBody] WaterQualityRequestDto dto)
    {
        var result = await _waterQualityService.CreateWaterQuality(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WaterQualityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaterQualityById(Guid id)
    {
        var result = await _waterQualityService.GetWaterQualityById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WaterQualityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateWaterQuality(Guid id, [FromBody] WaterQualityRequestDto dto)
    {
        var result = await _waterQualityService.UpdateWaterQuality(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteWaterQuality(Guid id)
    {
        await _waterQualityService.DeleteWaterQuality(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}