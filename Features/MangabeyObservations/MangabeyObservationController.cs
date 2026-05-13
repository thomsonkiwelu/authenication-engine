using conservation_backend.Features.MangabeyObservations.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.MangabeyObservations;

[Authorize]
[ApiController]
[Route("api/mangabey-observations")]
public class MangabeyObservationController(IMangabeyObservationService mangabeyObservationService) : ControllerBase
{
    private readonly IMangabeyObservationService _mangabeyObservationService = mangabeyObservationService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<MangabeyObservationResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] MangabeyObservationPaginationDto dto)
    {
        var pagedResult = await _mangabeyObservationService.GetMangabeyObservations(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetMangabeyObservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] MangabeyObservationRequestDto dto)
    {
        var result = await _mangabeyObservationService.CreateMangabeyObservation(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetMangabeyObservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mangabeyObservationService.GetMangabeyObservationById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetMangabeyObservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] MangabeyObservationRequestDto dto)
    {
        var result = await _mangabeyObservationService.UpdateMangabeyObservation(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mangabeyObservationService.DeleteMangabeyObservation(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}