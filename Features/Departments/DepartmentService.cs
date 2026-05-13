using authentication_engine.Features.Auth.Interfaces;
using authentication_engine.Features.Departments.Interfaces;
using authentication_engine.Shared;
using MapsterMapper;

namespace authentication_engine.Features.Departments
{
    public class DepartmentService(IDepartmentRepository repository, IMapper mapper, IPasswordService passwordService) : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<PagedList<DepartmentResponseDto>> GetAllDepartmentsData(DepartmentPaginationDto dto)
        {
            var pagedData = await _departmentRepository.GetPagedData(dto);

            var startIndex = (pagedData.Page - 1) * pagedData.PageSize + 1;

            var dtoList = _mapper.Map<List<DepartmentResponseDto>>(pagedData.Data)
                .Select((dto, index) => dto with { RowNumber = startIndex + index }).ToList();

            return new PagedList<DepartmentResponseDto>(
                items: dtoList,
                page: pagedData.Page,
                pageSize: pagedData.PageSize,
                totalCount: pagedData.TotalCount
            );
        }

        public async Task<DepartmentDto> CreateDepartment(DepartmentRequest dto)
        {
            var department = _mapper.Map<Department>(dto);

            var createdDepatment = await _departmentRepository.Create(department);

            var responseDto = _mapper.Map<DepartmentDto>(createdDepatment);

            return responseDto;
        }

        public async Task<DepartmentWithOfficeDto> GetDepartmentById(Guid id)
        {
            var department = await _departmentRepository.GetById(id);

            var result = _mapper.Map<DepartmentWithOfficeDto>(department);

            return result;
        }

        public async Task<DepartmentDto> UpdateDepartment(Guid id, DepartmentRequest dto)
        {
            var department = _mapper.Map<Department>(dto);

            var updatedDepartment = await _departmentRepository.Update(id, department);

            var responseDto = _mapper.Map<DepartmentDto>(updatedDepartment);

            return responseDto;
        }

        public async Task<bool> DeleteDepartment(Guid id)
        {
            return await _departmentRepository.Delete(id);
        }
        
        public async Task<bool> AssignStaffToDepartment(AssignStaffToDepartmentDto dto)
        {
            dto.Password = _passwordService.HashPassword("Tanapa@2026");
            
            return await _departmentRepository.AssignStaffToDepartment(dto);
        }
        
        public async Task<bool> UnassignStaffFromDepartment(UnassignStaffFromDepartmentDto dto)
        {
            return await _departmentRepository.UnassignStaffFromDepartment(dto);
        }
        
        public async Task<bool> UpdateAssignStaffDepartment(UpdateAssignStaffDepartmentDto dto)
        {
            var departmentStaff = _mapper.Map<DepartmentStaff>(dto);
            
            return await _departmentRepository.UpdateAssignStaffDepartment(departmentStaff);
        }
    }
}
