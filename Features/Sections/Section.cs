using conservation_backend.Features.Departments;
using conservation_backend.Features.Offices;
using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Features.Sections
{
    public class Section : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public Department Department { get; set; } = null!;
        public Guid DepartmentId { get; set; }

        public Office Office { get; set; } = null!;
        public Guid OfficeId { get; set; }

    }
}
