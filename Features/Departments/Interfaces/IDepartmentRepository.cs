using authentication_engine.Shared;

namespace authentication_engine.Features.Departments.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<PagedList<Department>> GetPagedData(DepartmentPaginationDto dto);

        Task<Department> Create(Department department);

        Task<Department> GetById(Guid id);

        Task<Department> Update(Guid id, Department department);

        Task<bool> Delete(Guid id);
        
        Task<bool> AssignStaffToDepartment(AssignStaffToDepartmentDto dto);
        
        Task<bool> UnassignStaffFromDepartment(UnassignStaffFromDepartmentDto dto);
        
        Task<bool> UpdateAssignStaffDepartment(DepartmentStaff departmentStaff);
    }
}
