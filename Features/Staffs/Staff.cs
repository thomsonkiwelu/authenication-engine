using System.ComponentModel.DataAnnotations;
using conservation_backend.Features.Ranks;
using conservation_backend.Shared;

namespace conservation_backend.Features.Staffs
{
    public class Staff : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [MaxLength(255)]
        public string FirstName { get; set; } = string.Empty;
    
        [MaxLength(255)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
    
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    
        [MaxLength(5)]
        public string Status { get; set; } = string.Empty;

        public Rank Rank { get; set; } = null!;
        
        public Guid RankId { get; set; }
    }

}
