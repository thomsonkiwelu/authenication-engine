using conservation_backend.Features.Offices;
using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Staffs;

namespace conservation_backend.Features.Departments
{
    public class Department : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public Guid? ReportingTo { get; set; }

        public Office Office { get; set; } = null!;
        public Guid OfficeId { get; set; }

    }
    
    public class DepartmentStaff : BaseEntity
    {
        [MaxLength(50)]
        public string ModelType { get; set; } = string.Empty;
        
        public Guid DepartmentId {  get; set; }

        public Staff Staff { get; set; } = null!;
        
        public Guid StaffId { get; set; }
        
        public Office Office { get; set; } = null!;
        
        public Guid OfficeId { get; set; }
    }
    
    public class DepartmentStaffHistory : BaseEntity
    {
        public Guid Id { get; set; }
        
        public Guid DepartmentId {  get; set; }

        public Staff Staff { get; set; } = null!;
        
        public Guid StaffId { get; set; }
        
        [MaxLength(50)]
        public string ModelType { get; set; } = string.Empty;
    }
}
