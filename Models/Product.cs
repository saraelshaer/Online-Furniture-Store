using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "❗Name is required.")]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required(ErrorMessage = "❗Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "❗Price must be a positive number.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "❗StockQuantity is required.")]
        public int StockQuantity { get; set; }

      
        [StringLength(255, ErrorMessage = "❗Image file name cannot be longer than 255 characters.")]
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "❗Invalid image file format. Only .jpg, .jpeg, .png, and .gif are allowed.")]
        public string ImageFileName { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual  Category Category { get; set; }
        [NotMapped]
        public IFormFile ProductImage { get; set; }

       
        public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

        public virtual ICollection<Review> Reviews { get; set; }=new List<Review>();

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }= new List<OrderProduct>();

        public virtual ICollection<WishListProduct> WishListProducts { get; set; } = new List<WishListProduct>();

    }
}
