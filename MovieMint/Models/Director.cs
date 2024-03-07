using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MovieMint.Models
{
    [Table("Directors")]
    public class Director
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime? LastUpdatedDate { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}
