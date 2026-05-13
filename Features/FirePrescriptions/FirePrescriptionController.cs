using conservation_backend.Features.FirePrescriptions.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.FirePrescriptions;

[Authorize]
[ApiController]
[Route("api/fire-prescriptions")]

public class FirePrescriptionController(IFirePrescriptionService firePrescriptionService): ControllerBase
{
    private readonly IFirePrescriptionService _firePrescriptionService = firePrescriptionService;
    
    [HttpGet]
    [ProducesResponseType(typeof(ResponseWithPagination<List<FirePrescriptionResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllData([FromQuery] FirePrescriptionPaginationDto dto)
    {
        var pagedResult = await _firePrescriptionService.GetFirePrescriptions(dto);

        return Ok(ApiHttpResponse.Page(pagedResult));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseWithData<GetFirePrescriptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FirePrescriptionRequestDto dto)
    {
        var result = await _firePrescriptionService.CreateFirePrescription(dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetFirePrescriptionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _firePrescriptionService.GetFirePrescriptionById(id);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseWithData<GetFirePrescriptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] FirePrescriptionRequestDto dto)
    {
        var result = await _firePrescriptionService.UpdateFirePrescription(id, dto);

        return Ok(ApiHttpResponse.Data(result));
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _firePrescriptionService.DeleteFirePrescription(id);

        return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
    }
}