using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "❗Name is required.")]
        [MinLength(3, ErrorMessage = "❗Name  must be at least 3 characters long.")]
        [MaxLength(15)]
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
