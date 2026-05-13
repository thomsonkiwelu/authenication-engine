using System.ComponentModel.DataAnnotations;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Ranks
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
