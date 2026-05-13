using conservation_backend.Features.Permissions.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Permissions
{
    [Authorize]
    [ApiController]
    [Route("api/permissions")]
    public class PermissionController(IPermissionService permissionService) : ControllerBase
    {
        private readonly IPermissionService _permissionService = permissionService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<PermissionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPermissions(
            [FromQuery] PermissionPaginationDto dto
        )
        {
            var pagedResult = await _permissionService.GetAllPermissionsData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<PermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionRequest dto)
        {
            var createdPermission = await _permissionService.CreatePermission(dto);

            return Ok(ApiHttpResponse.Data(createdPermission));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissionById(Guid id)
        {
            var role = await _permissionService.GetPermissionById(id);

            return Ok(ApiHttpResponse.Data(role));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<PermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] PermissionRequest dto)
        {
            var role = await _permissionService.UpdatePermission(id, dto);

            return Ok(ApiHttpResponse.Data(role));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            await _permissionService.DeletePermission(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }

        [HttpGet("models/{slugName}")]
        [ProducesResponseType(typeof(ResponseWithData<PermissionGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissionsGroupedBySystemModel(string slugName)
        {
            var permissions = await _permissionService.GetPermissionsGroupedBySystemModel(slugName);

            return Ok(ApiHttpResponse.Data(permissions));
        }
        
        [HttpGet("models")]
        [ProducesResponseType(typeof(ResponseWithData<String>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSystemModels()
        {
            var models = await _permissionService.GetAllSystemModels();

            return Ok(ApiHttpResponse.Data(models));
        }
    }
}
