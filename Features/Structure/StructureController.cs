using conservation_backend.Features.Structure.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Structure
{
    [Authorize]
    [ApiController]
    [Route("api/structures")]
    public class StructureController(IStructureService structureService) : ControllerBase
    {
        private readonly IStructureService _structureService = structureService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<StructureDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStructures([FromQuery] PaginationDto dto)
        {
            var pagedResult = await _structureService.GetAllStructuresData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<StructureDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStructure([FromBody] StructureRequest dto)
        {
            var createdStructure = await _structureService.CreateStructure(dto);

            return Ok(ApiHttpResponse.Data(createdStructure));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<StructureWithOfficeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStructureById(Guid id)
        {
            var structure = await _structureService.GetStructureById(id);

            return Ok(ApiHttpResponse.Data(structure));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<StructureDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStructure(Guid id, [FromBody] StructureRequest dto)
        {
            var structure = await _structureService.UpdateStructure(id, dto);

            return Ok(ApiHttpResponse.Data(structure));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteStructure(Guid id)
        {
            await _structureService.DeleteStructure(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
