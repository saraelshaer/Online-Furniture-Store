using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class UserRole
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
