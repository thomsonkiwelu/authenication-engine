using System.ComponentModel.DataAnnotations;
using authentication_engine.Features.Parks;
using authentication_engine.Features.Structure;
using authentication_engine.Shared.Entities;

namespace authentication_engine.Features.Offices
{
    public class Office : BaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string? Code { get; set; } = string.Empty;

        public int ParentOffice { get; set; }

        public int HeadOfOffice { get; set; }
        
        public Park? Park { get; set; } = null;
        public Guid? ParkId { get; set; }
        
        public Guid StructureId { get; set; }

        public StructureEntity Structure { get; set; } = null!;

    }
}
