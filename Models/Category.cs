using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "❗Name is required.")]
        [MinLength(3 , ErrorMessage = "❗Name must be at least 3 characters long.")]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required(ErrorMessage = "❗Description is required.")]
        [MaxLength(100)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;



        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
