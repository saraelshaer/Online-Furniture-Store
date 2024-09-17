using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;


        public virtual ICollection<Product> Products { get; set; }
    }
}
