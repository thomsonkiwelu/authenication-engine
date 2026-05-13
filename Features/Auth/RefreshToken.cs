using conservation_backend.Features.Users;
using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Features.Auth
{
    public class RefreshToken : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public User User { get; set; } = null!;
        public Guid UserId { get; set; }

    }
}
