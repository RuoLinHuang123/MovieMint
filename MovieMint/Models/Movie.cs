
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MovieMint.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Url]
        public string PosterLink { get; set; } = null!;

        [Required]
        [StringLength(250)]
        public string SeriesTitle { get; set; } = null!;

        [Required]
        public int ReleasedYear { get; set; }

        [Required]
        public int Runtime { get; set; }

        [Required]
        [Range(0, 10)]
        [Column(TypeName = "decimal(2, 1)")]
        public decimal IMDBRating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Overview { get; set; } = null!;

        [Range(0, 100)]
        public int? MetaScore { get; set; }

        [Required]
        public int NoOfVotes { get; set; }

        public long? Gross { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get;set; }

        public int? CertificateId { get; set; }

        [Required]
        public int DirectorId { get; set; }

        public Certificate? Certificate { get; set; }

        public Director? Director { get; set; }

        public ICollection<Movies_Genres>? Movies_Genres { get; set; }

        public ICollection<Movies_Stars>? Movies_Stars { get; set; }
    }
}
