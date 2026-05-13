using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Parks;
using conservation_backend.Features.Staffs;

namespace conservation_backend.Features.Users
{
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        
        public Staff Staff { get; set; } = null!;
        
        public Guid StaffId { get; set; }
    }
    
    public class UserPark : BaseEntity
    {
        public User User { get; set; } = null!;
        
        public Guid UserId { get; set; }
        
        public Guid ParkId { get; set; }
        
        public Park Park { get; set; } = null!;
    }

}
