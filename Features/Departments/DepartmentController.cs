using conservation_backend.Features.Departments.Interfaces;
using conservation_backend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conservation_backend.Features.Departments
{
    [Authorize]
    [ApiController]
    [Route("api/departments")]
    public class DepartmentController(IDepartmentService departmentService) : ControllerBase
    {
        private readonly IDepartmentService _departmentService = departmentService;

        [HttpGet]
        [ProducesResponseType(typeof(ResponseWithPagination<List<DepartmentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDepartments([FromQuery] DepartmentPaginationDto dto)
        {
            var pagedResult = await _departmentService.GetAllDepartmentsData(dto);

            return Ok(ApiHttpResponse.Page(pagedResult));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseWithData<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentRequest dto)
        {
            var result = await _departmentService.CreateDepartment(dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<DepartmentWithOfficeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var result = await _departmentService.GetDepartmentById(id);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseWithData<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment(Guid id, [FromBody] DepartmentRequest dto)
        {
            var result = await _departmentService.UpdateDepartment(id, dto);

            return Ok(ApiHttpResponse.Data(result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            await _departmentService.DeleteDepartment(id);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Deleted));
        }
        
        [HttpPost("assign-staff")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignStaffToDepartment(AssignStaffToDepartmentDto dto)
        {
            await _departmentService.AssignStaffToDepartment(dto);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Ok));
        }
        
        [HttpPost("unassign-staff")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnassignStaffFromDepartment(UnassignStaffFromDepartmentDto dto)
        {
            await _departmentService.UnassignStaffFromDepartment(dto);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Ok));
        }
        
        [HttpPost("reassign-staff")]
        [ProducesResponseType(typeof(ResponseWithMessage), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAssignStaffDepartment(UpdateAssignStaffDepartmentDto dto)
        {
            await _departmentService.UpdateAssignStaffDepartment(dto);

            return Ok(ApiHttpResponse.Message(ResponseMessages.Ok));
        }

    }
}
