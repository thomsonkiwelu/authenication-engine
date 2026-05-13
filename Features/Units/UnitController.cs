using authentication_engine.Features.Units.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.Units
{
    [Authorize]
    [ApiController]
    [Route("api/units")]
    public class UnitController(IUnitService unitService) : ControllerBase
    {
        private readonly IUnitService _unitService = unitService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<UnitDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUnits([FromQuery] UnitPaginationDto dto)
        {
            var pagedResult = await _unitService.GetAllUnitsData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<UnitDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUnit([FromBody] UnitRequest dto)
        {
            var result = await _unitService.CreateUnit(dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<UnitDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnitById(Guid id)
        {
            var result = await _unitService.GetUnitById(id);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<UnitDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUnit(Guid id, [FromBody] UnitRequest dto)
        {
            var result = await _unitService.UpdateUnit(id, dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUnit(Guid id)
        {
            await _unitService.DeleteUnit(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
        
        [HttpGet("organization-context/{officeId}")]
        [ProducesResponseType(typeof(ResponseWithData<UnitsWithDepartmentAndSectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUnitsWithDepartmentAndSection(Guid officeId)
        {
            var result = await _unitService.GetUnitsWithDepartmentAndSection(officeId);
            
            return Ok(ApiHttpResponse.Data(result));
        }
    }
}
