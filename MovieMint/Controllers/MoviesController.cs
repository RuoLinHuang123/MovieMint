using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMint.Constants;
using MovieMint.DTO;
using MovieMint.Models;
using System.Linq.Dynamic.Core;



namespace MovieMint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(
            ApplicationDbContext context,
            ILogger<MoviesController> logger)

        {
            _context = context;
            _logger = logger;
        }
        
        [HttpGet(Name = "GetMovies")]
        public async Task<RestDTO<Movie[]>> Get(
                [FromQuery] RequestDTO<MovieDTO> input
                )
        {
            var query = _context.Movies.AsQueryable();
            if(!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(b => b.SeriesTitle.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}").Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            return new RestDTO<Movie[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount, 
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Movies",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateMovie")]
        public async Task<RestDTO<Movie?>> Post(MovieDTO model)
        {
            var movie = await _context.Movies
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();
            if (movie != null)
            {
                if (!string.IsNullOrEmpty(model.SeriesTitle))
                    movie.SeriesTitle = model.SeriesTitle;
                if (model.ReleasedYear.HasValue && model.ReleasedYear.Value > 0)
                    movie.ReleasedYear = model.ReleasedYear.Value;
                if (!string.IsNullOrEmpty(model.PosterLink) && Uri.IsWellFormedUriString(model.PosterLink, UriKind.Absolute))
                    movie.PosterLink = model.PosterLink;
                if (model.Runtime.HasValue && model.Runtime.Value > 0)
                    movie.Runtime = model.Runtime.Value;
                if (model.IMDBRating.HasValue && model.IMDBRating.Value >= 0 && model.IMDBRating.Value <= 10)
                    movie.IMDBRating = model.IMDBRating.Value;
                if (!string.IsNullOrEmpty(model.Overview))
                    movie.Overview = model.Overview;
                if (model.MetaScore.HasValue && model.MetaScore.Value >= 0 && model.MetaScore.Value <= 100)
                    movie.MetaScore = model.MetaScore.Value;
                if (model.NoOfVotes.HasValue && model.NoOfVotes.Value >= 0)
                    movie.NoOfVotes = model.NoOfVotes.Value;
                if (model.Gross.HasValue && model.Gross.Value >= 0)
                    movie.Gross = model.Gross.Value;
                movie.LastUpdatedDate = DateTime.Now;
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Movie?>()
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Movies",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpDelete(Name = "DeleteMovie")]
        public async Task<RestDTO<Movie?>> Delete(int id)
        {
            var movie = await _context.Movies
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Movie?>()
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Movies",
                                id,
                                Request.Scheme)!,
                            "self",
                            "DELETE"),
                }
            };
        }



    }
}
