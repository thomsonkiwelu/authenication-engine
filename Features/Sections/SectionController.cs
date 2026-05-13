using conservation_backend.Features.Sections.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Sections
{
    [Authorize]
    [ApiController]
    [Route("api/sections")]
    public class SectionController(ISectionService sectionService) : ControllerBase
    {
        private readonly ISectionService _sectionService = sectionService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<SectionResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSections([FromQuery] SectionPaginationDto dto)
        {
            var pagedResult = await _sectionService.GetAllSectionsData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<SectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSection([FromBody] SectionRequest dto)
        {
            var result = await _sectionService.CreateSection(dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<SectionWithDepartmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionById(Guid id)
        {
            var result = await _sectionService.GetSectionById(id);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<SectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSection(Guid id, [FromBody] SectionRequest dto)
        {
            var result = await _sectionService.UpdateSection(id, dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSection(Guid id)
        {
            await _sectionService.DeleteSection(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
        
        [HttpGet("organization-context/{officeId}")]
        [ProducesResponseType(typeof(ResponseWithData<SectionAndDepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSectionsAndDepartment(Guid officeId)
        {
            var result = await _sectionService.GetSectionsAndDepartment(officeId);
            
            return Ok(ApiHttpResponse.Data(result));
        }
    }
}
