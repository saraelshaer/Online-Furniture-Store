using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
