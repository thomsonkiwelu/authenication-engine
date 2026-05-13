using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Offices;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Structure
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
