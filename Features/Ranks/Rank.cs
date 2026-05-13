using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Features.Ranks
{
    public class Rank : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        public int Level { get; set; }
    }
}
