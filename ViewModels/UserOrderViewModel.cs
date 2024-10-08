using FurnitureStore.Enums;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.ViewModels
{
    public class UserOrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "❗Order status is required.")]

        [EnumDataType(typeof(OrderStatus), ErrorMessage = "❗Invalid order status value.")]
        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "❗Total amount is required.")]
        public decimal TotalAmount { get; set; } = 0;

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

        [Required(ErrorMessage = "❗First name is required.")]
        [MaxLength(30, ErrorMessage = "❗First name cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "❗First name  must be at least 3 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "❗Last name is required.")]
        [MaxLength(30, ErrorMessage = "❗Last name cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "❗Last name  must be at least 3 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "❗Email Address is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "❗Please enter a valid email address (e.g., example@example.com).")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "❗Email already exists.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "❗Phone Number is required.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "❗Invalid phone number format.")]
        [MinLength(10, ErrorMessage = "❗Phone number must be at least 10 digits.")]
        [MaxLength(15, ErrorMessage = "❗Phone number cannot be longer than 15 digits.")]
        public string Phone { get; set; }


        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
