using conservation_backend.Features.VillageContexts.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.VillageContexts;

[Authorize]
[ApiController]
[Route("api/village-contexts")]
public class VillageContextController(IVillageContextService villageContextService) : ControllerBase
{
    private readonly IVillageContextService _villageContextService = villageContextService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<VillageContextResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] VillageContextPaginationDto dto)
    {
        var pagedResult = await _villageContextService.GetAllVillageContexts(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<VillageContextDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] VillageContextRequestDto dto)
    {
        var result = await _villageContextService.CreateVillageContext(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<VillageContextDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _villageContextService.GetVillageContextById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<VillageContextDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] VillageContextRequestDto dto)
    {
        var result = await _villageContextService.UpdateVillageContext(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _villageContextService.DeleteVillageContext(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
    [HttpPost("issues")]
    [ProducesResponseType(typeof(ResponseWithData<VillageContextDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVillageIssue([FromBody] VillageIssueRequestDto dto)
    {
        var result = await _villageContextService.CreateVillageIssue(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}/issues")]
    [ProducesResponseType(typeof(ResponseWithData<GetVillageIssueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVillageIssue(Guid id)
    {
        var result = await _villageContextService.GetVillageIssueById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("issues")]
    [ProducesResponseType(typeof(ResponseWithData<GetVillageIssueDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVillageIssue([FromBody] VillageIssueRequestDto dto)
    {
        var result = await _villageContextService.UpdateVillageIssue(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
}