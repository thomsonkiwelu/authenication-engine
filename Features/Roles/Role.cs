using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Permissions;
using authentication_engine.Features.Users;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Roles
{
    public class Role : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public class RolePermission : BaseEntity
    {
        [MaxLength(255)]
        public string ModuleName { get; set; } = string.Empty;
        
        public Guid RoleId { get; set; }

        public Guid PermissionId {  get; set; }

        public Role Role { get; set; } = null!;

        public PermissionEntity Permission { get; set; } = null!;
    }

    public class RoleUser : BaseEntity
    {
        public Guid RoleId { get; set; }

        public Guid UserId { get; set; }

        public Role Role { get; set; } = null!;

        public User User { get; set; } = null!;
    }

}
