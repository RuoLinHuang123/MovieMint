using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class GenreDTO
    {
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
