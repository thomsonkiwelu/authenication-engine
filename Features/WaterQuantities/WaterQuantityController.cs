using conservation_backend.Features.WaterQuantities.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.WaterQuantities;

[Authorize]
[ApiController]
[Route("api/water-quantities")]

public class WaterQuantityController(IWaterQuantityService waterQuantityService): ControllerBase
{
    private readonly IWaterQuantityService _waterQuantityService = waterQuantityService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<WaterQuantityRequestDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] WaterQuantityPaginationDto dto)
    {
        var pagedResult = await _waterQuantityService.GetAllWaterQuantitiesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<WaterQuantityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] WaterQuantityRequestDto dto)
    {
        var result = await _waterQuantityService.CreateWaterQuantity(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WaterQuantityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _waterQuantityService.GetWaterQuantityById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<WaterQuantityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] WaterQuantityRequestDto dto)
    {
        var result = await _waterQuantityService.UpdateWaterQuantity(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _waterQuantityService.DeleteWaterQuantity(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}