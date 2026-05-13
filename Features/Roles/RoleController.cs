using System.Security.Claims;
using conservation_backend.Features.Roles.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Roles
{
    [Authorize]
    [ApiController]
    [Route("api/roles")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<RoleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoles(
            [FromQuery] PaginationDto dto
        )
        {
            var pagedResult = await _roleService.GetAllRolesData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoles([FromBody] RoleRequest dto)
        {
            var createdRole = await _roleService.CreateRoles(dto);

            return Ok(ApiHttpResponse.Data(createdRole));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<RoleWithPermissionsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRolesById(Guid id)
        {
            var role = await _roleService.GetRolesById(id);

            return Ok(ApiHttpResponse.Data(role));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRoles(Guid id, [FromBody] RoleRequest dto)
        {
            var role = await _roleService.UpdateRoles(id, dto);

            return Ok(ApiHttpResponse.Data(role));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteRoles(Guid id)
        {
            await _roleService.DeleteRoles(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }

        [HttpPost("assign-permissions")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRolePermissions([FromBody] AssignRolePermissionRequest dto)
        {
            await _roleService.UpdateRolePermissions(dto);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Success));
        }
        
        [HttpPost("assign-user")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserRequest dto)
        {
            await _roleService.AssignRoleToUser(dto);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Success));
        }
        
        [HttpPost("unassign-user")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnassignRoleToUser([FromBody] AssignRoleToUserRequest dto)
        {
            await _roleService.UnassignRoleToUser(dto);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Success));
        }
    }
}
