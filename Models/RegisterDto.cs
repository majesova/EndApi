using System.ComponentModel.DataAnnotations;

namespace EndApi.Models
{
    public class RegisterDto
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        [Required]
        [StringLength(300)]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}