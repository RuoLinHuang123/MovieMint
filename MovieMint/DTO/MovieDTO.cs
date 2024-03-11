using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class MovieDTO
    {
        [Required]
        public int Id { get; set; }

        public string? SeriesTitle { get; set; }

        public int? ReleasedYear { get; set; }

        public string? PosterLink { get; set; }

        public int? Runtime { get; set; }

        public decimal? IMDBRating { get; set; }

        public string? Overview { get; set; }

        public int? MetaScore { get; set; }

        public int? NoOfVotes { get; set; }

        public long? Gross { get; set; }
    }
}
