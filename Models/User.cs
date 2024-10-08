using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "❗First name is required.")]
        [MaxLength(30, ErrorMessage = "❗First name cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "❗First name  must be at least 3 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "❗Last name is required.")]
        [MaxLength(30, ErrorMessage = "❗Last name cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "❗Last name  must be at least 3 characters long.")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "❗Password is required")]
        [MinLength(8, ErrorMessage = "❗Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "❗Please confirm your password.")]
        [Compare("Password")]
        [MinLength(8, ErrorMessage = "❗Confirm password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "❗Email Address is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "❗Please enter a valid email address (e.g., example@example.com).")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "❗Email already exists.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "❗Phone Number is required.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "❗Invalid phone number format.")]
        [MinLength(10, ErrorMessage = "❗Phone number must be at least 10 digits.")]
        [MaxLength(15, ErrorMessage = "❗Phone number cannot be longer than 15 digits.")]
        public string Phone { get; set; }


        [StringLength(255, ErrorMessage = "❗Image file name cannot be longer than 255 characters.")]
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "❗Invalid image file format. Only .jpg, .jpeg and .png are allowed.")]
        public string ImageFileName { get; set; } = "defaultUserImage.jpg";


        [MaxLength(15)]
        public string Country { get; set; }

       
        [MaxLength(15)]
        public string State { get; set; }

       
        [MaxLength(15)]
        public string City { get; set; }

       
        [MaxLength(15)]
        public string ZipCode { get; set; }

        public bool IsActive { get; set; } = true;


        public virtual Cart Cart { get; set; }

        public virtual WishList WishList { get; set; } 

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }

}
