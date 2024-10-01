using FurnitureStore.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status value.")]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int TotalAmount { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
