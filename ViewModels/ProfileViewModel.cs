using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(15, ErrorMessage = "First name cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "First name  must be at least 3 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(15, ErrorMessage = "Last name cannot exceed 15 characters.")]
        [MinLength(3, ErrorMessage = "Last name  must be at least 3 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., example@example.com).")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email already exists.")]
        public string Email { get; set; }


        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        [MinLength(10, ErrorMessage = "Phone number must be at least 10 digits.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot be longer than 15 digits.")]
        public string Phone { get; set; }

        [MaxLength(15)]
        public string Country { get; set; }


        [MaxLength(15)]
        public string State { get; set; }


        [MaxLength(15)]
        public string City { get; set; }


        [MaxLength(15)]
        public string ZipCode { get; set; }

        [StringLength(255, ErrorMessage = "Image file name cannot be longer than 255 characters.")]
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "Invalid image file format. Only .jpg, .jpeg and .png are allowed.")]
        public string ImageFileName { get; set; }
        public IFormFile ProfileImage { get; set; }


    }
}
