using conservation_backend.Features.RareEndangered.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.RareEndangered;

[Authorize]
[ApiController]
[Route("api/rare-endangered-species")]
public class RareEndangeredSpeciesController(IRareEndangeredSpeciesService rareEndangeredSpeciesService) : ControllerBase
{
    private readonly IRareEndangeredSpeciesService _rareEndangeredSpeciesService = rareEndangeredSpeciesService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<RareEndangeredSpeciesResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] RareEndangeredSpeciesPaginationDto dto)
    {
        var pagedResult = await _rareEndangeredSpeciesService.GetPagedRareEndangeredSpecies(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetRareEndangeredSpeciesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] RareEndangeredSpeciesRequestDto dto)
    {
        var result = await _rareEndangeredSpeciesService.CreateRareEndangeredSpecies(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetRareEndangeredSpeciesDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _rareEndangeredSpeciesService.GetRareEndangeredSpeciesById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetRareEndangeredSpeciesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] RareEndangeredSpeciesRequestDto dto)
    {
        var result = await _rareEndangeredSpeciesService.UpdateRareEndangeredSpecies(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _rareEndangeredSpeciesService.DeleteRareEndangeredSpecies(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}