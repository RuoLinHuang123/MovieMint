namespace MovieMint.Models.CSV
{
    public class MovieRecord
    {
        public string Poster_Link { get; set; } = null!;

        public string Series_Title { get; set; } = null!;

        public int Released_Year { get; set; }

        public string? Certificate {  get; set; }

        public string Runtime { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public decimal IMDB_Rating { get; set; }

        public string Overview { get; set; } = null!;

        public int? Meta_score { get; set; }

        public string Director { get; set; } = null!;

        public string Star1 { get; set; } = null!;

        public string Star2 { get; set; } = null!;

        public string Star3 { get; set; } = null!;

        public string Star4 { get; set; } = null!;

        public int No_of_Votes { get; set; }

        public string? Gross { get; set; }

    }
}
