using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "❗Email Address is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address (e.g., example@example.com).")]

        public string Email { get; set; }

        [Required(ErrorMessage = "❗Password is required")]
        [MinLength(8, ErrorMessage = "❗Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
