using System.ComponentModel.DataAnnotations;

namespace Finance.API.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        // Указываем желаемую роль: "Admin", "Manager", "User"
        public string Role { get; set; } = "User";
    }
}