using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Departments;
using authentication_engine.Features.Offices;
using authentication_engine.Features.Sections;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Units
{
    public class Unit : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public Department? Department { get; set; } = null;
        public Guid? DepartmentId { get; set; }

        public Section? Section { get; set; } = null;
        public Guid? SectionId { get; set; }

        public Office Office { get; set; } = null!;
        public Guid OfficeId { get; set; }
    }
}
