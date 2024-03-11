using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMint.Models;
using MovieMint.Models.CSV;
using System.Globalization;


namespace MovieMint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeedController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        private readonly ILogger<SeedController> _logger;

        public SeedController(
            ApplicationDbContext context,
            IWebHostEnvironment env,
            ILogger<SeedController> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        [HttpPut(Name = "Seed")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Put()
        {
            var config = new CsvConfiguration(CultureInfo.GetCultureInfo("en-US"))
            {
                HasHeaderRecord = true,
                Delimiter = ",",
            };
            using var reader = new StreamReader(
                System.IO.Path.Combine(_env.ContentRootPath, "Data/imdb_top_1000.csv"));
            using var csv = new CsvReader(reader, config);
            var existingMovies = await _context.Movies
                .ToDictionaryAsync(m => (m.SeriesTitle, m.ReleasedYear));
            var existingCertificates = await _context.Certificates
                .ToDictionaryAsync(c => c.Name);
            var existingDirectors = await _context.Directors
                .ToDictionaryAsync(d => d.Name);
            var existingGenres = await _context.Genres
                .ToDictionaryAsync(g => g.Name);
            var existingStars = await _context.Stars
                .ToDictionaryAsync(s => s.Name);
            var now = DateTime.Now;

            var records = csv.GetRecords<MovieRecord>();
            var skippedRows = 0;
            foreach (var record in records)
            {
                if (existingMovies.GetValueOrDefault((record.Series_Title, record.Released_Year)) != null)
                {
                    skippedRows++;
                    continue;
                }
                var director = existingDirectors.GetValueOrDefault(record.Director);

                if (director == null)
                {
                    director = new Director()
                    {
                        Name = record.Director,
                        CreatedDate = now,
                        LastUpdatedDate = now,
                    };
                    _context.Directors.Add(director);
                    existingDirectors.Add(record.Director, director);
                }
                Certificate? certificate = null;
                if (!string.IsNullOrEmpty(record.Certificate))
                {
                    certificate = existingCertificates.GetValueOrDefault(record.Certificate);
                    if (certificate == null)
                    {
                        certificate = new Certificate()
                        {
                            Name = record.Certificate,
                            CreatedDate = now,
                            LastUpdatedDate = now,
                        };
                        _context.Certificates.Add(certificate);
                        existingCertificates.Add(record.Certificate, certificate);
                    }
                }
                bool conversionSucceeded = long.TryParse(record.Gross, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out long result);
                long? grosslong = null;
                if (conversionSucceeded)
                {
                    grosslong = result;
                }
                string[] parts = record.Runtime.Split(' ');
                var movie = new Movie()
                {
                    PosterLink = record.Poster_Link,
                    SeriesTitle = record.Series_Title,
                    ReleasedYear = record.Released_Year,
                    Runtime = int.Parse(parts[0]),
                    IMDBRating = record.IMDB_Rating,
                    Overview = record.Overview,
                    MetaScore = record.Meta_score ?? null,
                    NoOfVotes = record.No_of_Votes,
                    Gross = grosslong,
                    Director = director,
                    Certificate = certificate ?? null,
                    CreatedDate = now,
                    LastUpdatedDate = now,
                };
                _context.Movies.Add(movie);

                if (!string.IsNullOrEmpty(record.Genre))
                {
                    foreach (var genreName in record.Genre
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Distinct(StringComparer.InvariantCultureIgnoreCase))
                    {
                        var genre = existingGenres.GetValueOrDefault(genreName);
                        if (genre == null)
                        {
                            genre = new Genre()
                            {
                                Name = genreName,
                                CreatedDate = now,
                                LastUpdatedDate = now
                            };
                            _context.Genres.Add(genre);
                            existingGenres.Add(genreName, genre);
                        }
                        _context.Movies_Genres.Add(new Movies_Genres()
                        {
                            Movie = movie,
                            Genre = genre,
                            CreatedDate = now
                        });
                    }
                }
                string[] Stars = new string[] { record.Star1, record.Star2, record.Star3, record.Star4 };
                string[] uniqueStars = Stars.Distinct().ToArray();
                foreach (var starName in uniqueStars)
                {
                    if (!string.IsNullOrEmpty(starName))
                    {
                        var star = existingStars.GetValueOrDefault(starName);
                        if (star == null)
                        {
                            star = new Star()
                            {
                                Name=starName,
                                CreatedDate=now,
                                LastUpdatedDate = now
                            };
                            _context.Stars.Add(star);
                            existingStars.Add(starName, star);
                        }
                        _context.Movies_Stars.Add(new Movies_Stars()
                        {
                            Movie = movie,
                            Star = star,
                            CreatedDate = now
                        });
                    }

                }
            }
            await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                Movies = _context.Movies.Count(),
                Certificates= _context.Certificates.Count(),
                Genres = _context.Genres.Count(),
                Directors = _context.Directors.Count(),
                Stars = _context.Stars.Count(),
                SkippedRows = skippedRows
            });
        }
    }
}
 