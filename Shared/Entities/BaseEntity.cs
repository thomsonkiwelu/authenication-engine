using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using conservation_backend.Features.Users;

namespace conservation_backend.Shared
{
    public class BaseEntity
    {
        //INFO: Create
        public Guid? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")] 
        public User? Creator { get; set; } = null;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //INFO: Update
        public Guid? UpdatedBy { get; set; }
        
        [ForeignKey("UpdatedBy")]
        public User? Updater { get; set; } = null;
        
        public DateTime? UpdatedAt { get; set; }

        //INFO: Delete
        public DateTime? DeletedAt { get; set; }
    }
}
