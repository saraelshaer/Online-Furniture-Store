using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(100)]
        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
