using authentication_engine.Features.Offices.Interface;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.Offices
{
    [Authorize]
    [ApiController]
    [Route("api/offices")]
    public class OfficeController(IOfficeService officeService) : ControllerBase
    {
        private readonly IOfficeService _officeService = officeService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<OfficeDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOffice([FromQuery] OfficePaginationDto dto)
        {
            var pagedResult = await _officeService.GetAllOfficesData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<OfficeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeRequest dto)
        {
            var createdOffice = await _officeService.CreateOffice(dto);

            return Ok(ApiHttpResponse.Data(createdOffice));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<OfficeWithStructureDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOfficeById(Guid id)
        {
            var office = await _officeService.GetOfficeById(id);

            return Ok(ApiHttpResponse.Data(office));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<OfficeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOffice(Guid id, [FromBody] OfficeRequest dto)
        {
            var office = await _officeService.UpdateOffice(id, dto);

            return Ok(ApiHttpResponse.Data(office));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOffice(Guid id)
        {
            await _officeService.DeleteOffice(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
