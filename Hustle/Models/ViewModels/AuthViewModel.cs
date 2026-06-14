using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hustle.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        [StringLength(50, ErrorMessage = "First name should be between 3 and 50 characters.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "First name can only contain letters, spaces, apostrophes, and hyphens.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        [StringLength(50, ErrorMessage = "Last name should be between 3 and 50 characters.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Last name can only contain letters, spaces, apostrophes, and hyphens.")]
        public string LastName { get; set; }

        [DisplayName("Profile Picture")]
        [AllowedFileExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" }, ErrorMessage = "Only JPG, JPEG, PNG, or WEBP images are allowed.")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Profile picture must be less than 5MB.")]
        public IFormFile ImageUrl { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DisplayName("Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.", MinimumLength = 10)]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Please enter a valid South African phone number (e.g., 0712345678 or +27712345678).")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [DisplayName("Location (Township/City)")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.", MinimumLength = 2)]
        public string Location { get; set; }

        [DisplayName("Bio")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DisplayName("I'm looking for work")]
        public bool Role { get; set; }
    }

    public class LoginViewModel
    {
        public string ReturnUrl { get; set; } = "/";

        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [DisplayName("Remember Me?")]
        public bool RememberMe { get; set; }
    }

    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage ?? $"Only {string.Join(", ", _extensions)} files are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage ?? $"File size must be less than {_maxFileSize / (1024 * 1024)}MB.");
            }
            return ValidationResult.Success;
        }
    }
}
