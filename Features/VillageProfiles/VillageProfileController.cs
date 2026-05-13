using conservation_backend.Features.VillageProfiles.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.VillageProfiles;

[Authorize]
[ApiController]
[Route("api/village-profiles")]
public class VillageProfileController(IVillageProfileService villageProfileService) : ControllerBase
{
    private readonly IVillageProfileService _villageProfileService = villageProfileService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<VillageProfileResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] VillageProfilePaginationDto dto)
    {
        var pagedResult = await _villageProfileService.GetAllVillageProfilesData(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<VillageProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] VillageProfileRequestDto dto)
    {
        var result = await _villageProfileService.CreateVillageProfile(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<VillageProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _villageProfileService.GetVillageProfileById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<VillageProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] VillageProfileRequestDto dto)
    {
        var result = await _villageProfileService.UpdateVillageProfile(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _villageProfileService.DeleteVillageProfile(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
    
    [HttpGet("{id}/details")]
    [ProducesResponseType(typeof(ResponseWithData<GetVillageProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFullVillageProfileById(Guid id)
    {
        var result = await _villageProfileService.GetFullVillageProfileById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
}