using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
