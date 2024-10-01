using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FurnitureStore.Enums;

namespace FurnitureStore.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [StringLength(50)]
        [EnumDataType(typeof(PaymentStatus), ErrorMessage = "Invalid payment status value.")]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
