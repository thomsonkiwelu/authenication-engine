using System.ComponentModel.DataAnnotations.Schema;
using authentication_engine.Features.Users;

namespace authentication_engine.Shared.Entities
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
