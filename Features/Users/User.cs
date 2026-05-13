using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Parks;
using authentication_engine.Features.Staffs;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Users
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
