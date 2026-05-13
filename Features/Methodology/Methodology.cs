using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Features.Methodology
{
    public class Methodology
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(150)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime DeletedAt { get; set; }
    }
}
