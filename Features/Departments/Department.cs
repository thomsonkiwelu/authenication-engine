using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Offices;
using authentication_engine.Features.Staffs;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Departments
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
