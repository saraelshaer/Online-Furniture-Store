using FurnitureStore.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "❗Order status is required.")]
     
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "❗Invalid order status value.")]
        [Display(Name ="Order Status")]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "❗Total amount is required.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "❗Country is required.")]
        [MaxLength(15)]
        public string Country { get; set; }

        [Required(ErrorMessage = "❗State is required.")]
        [MaxLength(15)]
        public string State { get; set; }

        [Required(ErrorMessage = "❗City is required.")]
        [MaxLength(15)]
        public string City { get; set; }

        [Required(ErrorMessage = "❗Zip_Code is required.")]
        [MaxLength(15)]
        public string ZipCode { get; set; }

        
        public string Notes { get; set; }

       
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

     
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
