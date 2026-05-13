using conservation_backend.Features.InvasiveSpecies.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.InvasiveSpecies;

[Authorize]
[ApiController]
[Route("api/invasive-species")]
public class InvasiveController(IInvasiveService invasiveService): ControllerBase
{
    private readonly IInvasiveService _invasiveService = invasiveService;

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<InvasiveResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvasiveSpecies(
        [FromQuery] InvasivePaginationDto dto
    )
    {
        var pagedResult = await _invasiveService.GetPagedInvasiveSpecies(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetInvasiveSpeciesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInvasiveSpecies([FromBody] InvasiveRequestDto dto)
    {
        var result = await _invasiveService.CreateInvasiveSpecies(dto);
            
        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetInvasiveSpeciesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvasiveSpeciesById(Guid id)
    {
        var result = await _invasiveService.GetInvasiveSpeciesById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetInvasiveSpeciesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateInvasiveSpecies(Guid id, [FromBody] InvasiveRequestDto dto)
    {
        var result = await _invasiveService.UpdateInvasiveSpecies(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _invasiveService.DeleteInvasiveSpecies(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
}