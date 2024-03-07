using System.ComponentModel.DataAnnotations;

namespace MovieMint.Models
{
    public class Movies_Stars
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Key]
        [Required]
        public int StarId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public Movie? Movie { get; set; }

        public Star? Star { get; set; }
    }
}
