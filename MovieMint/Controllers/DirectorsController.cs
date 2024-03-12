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
    public class DirectorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<DirectorsController> _logger;

        public DirectorsController(
            ApplicationDbContext context,
            ILogger<DirectorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetDirectors")]
        public async Task<RestDTO<Director[]>> Get(
            [FromQuery] RequestDTO<DirectorDTO> input)
        {

            var query = _context.Directors.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(b => b.Name.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}").Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            return new RestDTO<Director[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Directors",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateDirector")]
        public async Task<RestDTO<Director?>> Post(DirectorDTO model)
        {
            var director = await _context.Directors
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();

            if (director != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    director.Name = model.Name;
                director.LastUpdatedDate = DateTime.Now;
                _context.Directors.Update(director);
                await _context.SaveChangesAsync();
            };

            return new RestDTO<Director?>()
            {
                Data = director,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Directors",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpDelete(Name = "DeleteDirector")]
        public async Task<RestDTO<Director?>> Delete(int id)
        {
            var director = await _context.Directors
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (director != null)
            {
                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Director?>()
            {
                Data = director,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Directors",
                                id,
                                Request.Scheme)!,
                            "self",
                            "DELETE"),
                }
            };


        }


    }
}
