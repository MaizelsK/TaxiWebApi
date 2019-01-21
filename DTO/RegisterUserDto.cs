using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Password must at least 4 characters")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
