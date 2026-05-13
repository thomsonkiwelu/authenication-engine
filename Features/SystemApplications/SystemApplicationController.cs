using authentication_engine.Features.SystemApplications.Interfaces;
using authentication_engine.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication_engine.Features.SystemApplications
{
    [Authorize]
    [ApiController]
    [Route("api/system-applications")]
    public class SystemApplicationController(ISystemApplicationService systemApplicationService) : ControllerBase
    {
        private readonly ISystemApplicationService _systemApplicationService = systemApplicationService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<SystemApplicationDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllData([FromQuery] PaginationDto dto)
        {
            var pagedResult = await _systemApplicationService.GetAllSystemApplications(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<SystemApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] SystemApplicationRequestDto dto)
        {
            var result = await _systemApplicationService.CreateSystemApplication(dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<SystemApplicationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _systemApplicationService.GetSystemApplicationById(id);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<SystemApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] SystemApplicationRequestDto dto)
        {
            var result = await _systemApplicationService.UpdateSystemApplication(id, dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _systemApplicationService.DeleteSystemApplication(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
    }
}
