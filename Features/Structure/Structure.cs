using conservation_backend.Features.Offices;
using conservation_backend.Shared;
using System.ComponentModel.DataAnnotations;

namespace conservation_backend.Features.Structure
{
    public class StructureEntity : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public int Level { get; set; }

        public ICollection<Office> Offices { get; set; } = new List<Office>();
    }
}
