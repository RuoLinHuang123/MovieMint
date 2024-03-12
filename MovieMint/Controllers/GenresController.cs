using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using MovieMint.DTO;
using MovieMint.Models;
using Microsoft.AspNetCore.Authorization;
using MovieMint.Constants;

namespace MovieMint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<GenresController> _logger;

        public GenresController(
            ApplicationDbContext context,
            ILogger<GenresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetGenres")]
        public async Task<RestDTO<Genre[]>> Get(
            [FromQuery] RequestDTO<GenreDTO> input)
        {
            var query = _context.Genres.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(b => b.Name.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}").Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            return new RestDTO<Genre[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Genres",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateGenre")]
        public async Task<RestDTO<Genre?>> Post(GenreDTO model)
        {
            var genre = await _context.Genres
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();

            if (genre != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    genre.Name = model.Name;
                genre.LastUpdatedDate = DateTime.Now;
                _context.Genres.Update(genre);
                await _context.SaveChangesAsync();
            };

            return new RestDTO<Genre?>()
            {
                Data = genre,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Genres",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpDelete(Name = "DeleteGenre")]
        public async Task<RestDTO<Genre?>> Delete(int id)
        {
            var genre = await _context.Genres
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Genre?>()
            {
                Data = genre,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Genres",
                                id,
                                Request.Scheme)!,
                            "self",
                            "DELETE"),
                }
            };
        }
    }
}
