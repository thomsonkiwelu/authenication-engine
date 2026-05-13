using conservation_backend.Config;
using conservation_backend.Features.HabitatManipulations.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.HabitatManipulations;

[Authorize]
[ApiController]
[Route("api/habitat-manipulations")]
public class HabitatManipulationController(IHabitatManipulationService habitatManipulationService): ControllerBase
{
    private readonly IHabitatManipulationService _habitatManipulationService = habitatManipulationService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<HabitatManipulationResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHabitatManipulations(
        [FromQuery] HabitatManipulationPaginationDto dto
    )
    {
        var pagedResult = await _habitatManipulationService.GetPagedHabitatManipulations(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetHabitatManipulationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHabitatManipulation([FromBody] HabitatManipulationRequestDto dto)
    {
        var result = await _habitatManipulationService.CreateHabitatManipulation(dto);
            
        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetHabitatManipulationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHabitatManipulationById(Guid id)
    {
        var result = await _habitatManipulationService.GetHabitatManipulationById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetHabitatManipulationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateHabitatManipulation(Guid id, [FromBody] HabitatManipulationRequestDto dto)
    {
        var result = await _habitatManipulationService.UpdateHabitatManipulation(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteHabitatManipulation(Guid id)
    {
        await _habitatManipulationService.DeleteHabitatManipulation(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}