using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Users;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.SystemApplications
{
    public class SystemApplication : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Url { get; set; } = string.Empty;
        
        public string ApiKey { get; set; } = string.Empty;
    }
    
    public class UserSystemApplication : BaseEntity
    {
        public User User { get; set; } = null!;
        
        public Guid UserId { get; set; }
        
        public Guid SystemApplicationId { get; set; }
        
        public SystemApplication SystemApplication { get; set; } = null!;
    }
}
