using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FurnitureStore.Validators
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           string password = value as string;
            if (password.Length < 8)
            {
                return new ValidationResult("❗Password must be at least 8 characters long.");
            }
            else if (!Regex.IsMatch(password,@"[A-Z]"))
            {
                return new ValidationResult("❗Password must contain at least one uppercase letter.");
            }
            else if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ValidationResult("❗Password must contain at least one lowercase letter.");
            }
            else if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return new ValidationResult("❗Password must contain at least one digit.");
            }
            else if (!Regex.IsMatch(password, @"[_!@#$%?]"))
            {
                return new ValidationResult("❗Password must contain at least one special character ( _,!, @, #, $, %, ?).");
            }
            return ValidationResult.Success;
        }
    }
}
