using conservation_backend.Shared;

namespace conservation_backend.Features.Departments.Interfaces
{
    public interface IDepartmentService
    {
        Task<PagedList<DepartmentResponseDto>> GetAllDepartmentsData(DepartmentPaginationDto dto);

        Task<DepartmentDto> CreateDepartment(DepartmentRequest dto);

        Task<DepartmentWithOfficeDto> GetDepartmentById(Guid id);

        Task<DepartmentDto> UpdateDepartment(Guid id, DepartmentRequest dto);

        Task<bool> DeleteDepartment(Guid id);
        
        Task<bool> AssignStaffToDepartment(AssignStaffToDepartmentDto dto);
        
        Task<bool> UnassignStaffFromDepartment(UnassignStaffFromDepartmentDto dto);
        
        Task<bool> UpdateAssignStaffDepartment(UpdateAssignStaffDepartmentDto dto);
    }
}
