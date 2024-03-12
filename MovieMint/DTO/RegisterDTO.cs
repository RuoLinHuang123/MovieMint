using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string? PhoneNumber { get; set; }
    }
}
