using System.ComponentModel.DataAnnotations;
using conservation_backend.Shared;

namespace conservation_backend.Features.Species
{
    public class Species : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string CommonName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ScientificName { get; set; } = string.Empty;

        public int Type { get; set; } //1=plant and 2=animals 3=birds
    }
}
