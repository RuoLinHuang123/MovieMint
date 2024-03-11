using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class DirectorDTO
    {
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
