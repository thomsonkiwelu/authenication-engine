using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Ranks;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Staffs
{
    public class Staff : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? MiddleName { get; set; } = string.Empty;
    
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
    
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    
        [MaxLength(5)]
        public string Status { get; set; } = string.Empty;

        public Rank Rank { get; set; } = null!;
        public Guid RankId { get; set; }
        
        public float? TnpNumber { get; set; }
    }

}
