using conservation_backend.Features.AerialCensuses.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.AerialCensuses;

[Authorize]
[ApiController]
[Route("api/aerial-censuses")]
public class AerialCensusController(IAerialCensusService aerialCensusService): ControllerBase
{
    private readonly IAerialCensusService _aerialCensusService = aerialCensusService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<AerialCensusResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAerialCensuses(
        [FromQuery] AerialCensusPaginationDto dto
    )
    {
        var pagedResult = await _aerialCensusService.GetPagedAerialCensuses(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetAerialCensusDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAerialCensus([FromBody] AerialCensusRequestDto dto)
    {
        var result = await _aerialCensusService.CreateAerialCensus(dto);
            
        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetAerialCensusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAerialCensusById(Guid id)
    {
        var result = await _aerialCensusService.GetAerialCensusById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetAerialCensusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAerialCensus(Guid id, [FromBody] AerialCensusRequestDto dto)
    {
        var result = await _aerialCensusService.UpdateAerialCensus(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAerialCensus(Guid id)
    {
        await _aerialCensusService.DeleteAerialCensus(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}