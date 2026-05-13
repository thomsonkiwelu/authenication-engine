using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Features.Permissions
{
    public class PermissionEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Action { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ModelType { get; set; } = string.Empty;

        public SystemModules.SystemModule SystemModule { get; set; } = null!;
        public Guid SystemModuleId { get; set; }
    }
}
