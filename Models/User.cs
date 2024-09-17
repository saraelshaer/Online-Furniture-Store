using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string LastName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [Compare("Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., example@example.com).")]
        public string Email { get; set; }


        [StringLength(255, ErrorMessage = "Image file name cannot be longer than 255 characters.")]
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "Invalid image file format. Only .jpg, .jpeg, .png, and .gif are allowed.")]
        public string ImageFileName { get; set; }

        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        [MinLength(10, ErrorMessage = "Phone number must be at least 10 digits.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot be longer than 15 digits.")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(15)]
        public string Country { get; set; }

        [Required]
        [MaxLength(15)]
        public string State { get; set; }

        [Required]
        [MaxLength(15)]
        public string City { get; set; }

        [Required]
        [MaxLength(15)]
        public string ZipCode { get; set; }

        public bool IsActive { get; set; } = true;


        public virtual Cart Cart { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }

}
