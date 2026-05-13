using conservation_backend.Features.Offices;
using conservation_backend.Shared;

namespace conservation_backend.Features.Departments
{
    public record DepartmentPaginationDto : PaginationDto
    {
        public string? OfficeId { get; init; }
    }

    public record DepartmentRequest(
        string Name,
        string Code,
        Guid OfficeId
    );

    public record DepartmentResponseDto(
        Guid Id,
        int RowNumber,
        string Name,
        string Code,
        Guid OfficeId,
        DateTime CreatedAt
        //string CreatedBy
    );

    public record DepartmentDto
    {
        public Guid Id { get; init; }
        
        public string Name { get; init; } = string.Empty;
        
        public string Code { get; init; } = string.Empty;
        
        public Guid OfficeId { get; init; }
        
        public DateTime CreatedAt { get; init; }
    }

    public record DepartmentWithOfficeDto : DepartmentDto
    {
        public OfficeDto? Office { get; init; }
    }
    
    public record AssignStaffToDepartmentDto
    {
        public Guid StaffId { get; set; }
        
        public Guid DepartmentId { get; set; }
        
        public Guid OfficeId { get; set; }
        
        public Guid RoleId { get; set; }
        
        public string ModelType { get; set; } = string.Empty;
        
        public Guid? CreatedBy { get; set; }
        
        public string? Password { get; set; } = string.Empty;
    }
    
    public record UnassignStaffFromDepartmentDto
    {
        public Guid StaffId { get; set; }
        
        public Guid OfficeId { get; set; }
        
        public Guid? CurrentUser { get; set; }
    }
    
    public record UpdateAssignStaffDepartmentDto
    {
        public Guid DepartmentId { get; set; }
        
        public Guid StaffId { get; set; }
        
        public string ModelType { get; set; }
    }
    
}
