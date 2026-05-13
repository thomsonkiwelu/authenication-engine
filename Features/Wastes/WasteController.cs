using conservation_backend.Features.Wastes.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Wastes
{
    [Authorize]
    [ApiController]
    [Route("api/wastes")]
    public class WasteController(IWasteService wasteService): ControllerBase
    {
        private readonly IWasteService _wasteService = wasteService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<WasteResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWastes(
            [FromQuery] WastePaginationDto dto
        )
        {
            var pagedResult = await _wasteService.GetAllWastesData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<WasteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWaste([FromBody] WasteRequestDto dto)
        {
            var result = await _wasteService.CreateWaste(dto);
            
            return Ok(ApiHttpResponse.Data(result));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<GetWasteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWasteById(Guid id)
        {
            var result = await _wasteService.GetWasteById(id);

            return Ok(ApiHttpResponse.Data(result));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<GetWasteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateWaste(Guid id, [FromBody] WasteRequestDto dto)
        {
            var result = await _wasteService.UpdateWaste(id, dto);

            return Ok(ApiHttpResponse.Data(result));
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteWaste(Guid id)
        {
            await _wasteService.DeleteWaste(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
