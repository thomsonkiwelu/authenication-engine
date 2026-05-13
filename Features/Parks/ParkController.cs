using conservation_backend.Features.Parks.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Parks;

[Authorize]
[ApiController]
[Route("api/parks")]
public class ParkController(IParkService parkService) : ControllerBase
{
    private readonly IParkService _parkService = parkService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<ParkResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetParks([FromQuery] ParkPaginationDto dto)
    {
        var pagedResult = await _parkService.GetAllParksData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<ParkDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePark([FromBody] ParkRequest dto)
    {
        var result = await _parkService.CreatePark(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<ParkDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetParkById(Guid id)
    {
        var result = await _parkService.GetParkById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<ParkDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePark(Guid id, [FromBody] ParkRequest dto)
    {
        var result = await _parkService.UpdatePark(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeletePark(Guid id)
    {
        await _parkService.DeletePark(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}