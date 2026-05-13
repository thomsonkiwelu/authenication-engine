using conservation_backend.Features.Vegetation.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Vegetation
{
    [Authorize]
    [ApiController]
    [Route("api/vegetations")]
    public class VegetationController(IVegetationService vegetationService): ControllerBase
    {
        private readonly IVegetationService _vegetationService = vegetationService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<VegetationResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVegetations(
            [FromQuery] VegetationPaginationDto dto
        )
        {
            var pagedResult = await _vegetationService.GetPagedVegetations(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<GetVegetationResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVegetation([FromBody] VegetationRequestDto dto)
        {
            var result = await _vegetationService.CreateVegetation(dto);
            
            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<GetVegetationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVegetationById(Guid id)
        {
            var vegetation = await _vegetationService.GetVegetationById(id);

            return Ok(ApiHttpResponse.Data(vegetation));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<GetVegetationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateVegetation(Guid id, [FromBody] VegetationRequestDto dto)
        {
            var result = await _vegetationService.UpdateVegetation(id, dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vegetationService.DeleteVegetation(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
