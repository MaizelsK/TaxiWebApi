using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class SignInDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
