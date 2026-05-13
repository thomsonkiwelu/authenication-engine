using conservation_backend.Features.WaterBodies.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.WaterBodies;

[Authorize]
[ApiController]
[Route("api/water-bodies")]
public class WaterBodyController(IWaterBodyService waterBodyService): ControllerBase
{
    private readonly IWaterBodyService _waterBodyService = waterBodyService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<WaterBodyResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllWaterBodies([FromQuery] WaterBodyPaginationDto dto)
    {
        var pagedResult = await _waterBodyService.GetAllWaterBodiesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<WaterBodyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWaterBody([FromBody] WaterBodyRequest dto)
    {
        var result = await _waterBodyService.CreateWaterBody(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WaterBodyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaterBodyById(Guid id)
    {
        var result = await _waterBodyService.GetWaterBodyById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WaterBodyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateWaterBody(Guid id, [FromBody] WaterBodyRequest dto)
    {
        var result = await _waterBodyService.UpdateWaterBody(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteWaterBody(Guid id)
    {
        await _waterBodyService.DeleteWaterBody(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}