using conservation_backend.Features.Divisions.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Divisions;

[Authorize]
[ApiController]
[Route("api/divisions")]
public class DivisionController(IDivisionService divisionService) : ControllerBase
{
    private readonly IDivisionService _divisionService = divisionService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<DivisionResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] DivisionPaginationDto dto)
    {
        var pagedResult = await _divisionService.GetAllDivisions(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<DivisionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DivisionRequest dto)
    {
        var result = await _divisionService.CreateDivision(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DivisionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _divisionService.GetDivisionById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<DivisionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] DivisionRequest dto)
    {
        var result = await _divisionService.UpdateDivision(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _divisionService.DeleteDivision(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}