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
    public class StarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<StarsController> _logger;

        public StarsController(
            ApplicationDbContext context,
            ILogger<StarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetStars")]
        public async Task<RestDTO<Star[]>> Get(
            [FromQuery] RequestDTO<StarDTO> input)
        {
            var query = _context.Stars.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(b => b.Name.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}").Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            return new RestDTO<Star[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Stars",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateStars")]
        public async Task<RestDTO<Star?>> Post(StarDTO model)
        {
            var star = await _context.Stars
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();

            if (star != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    star.Name = model.Name;
                star.LastUpdatedDate = DateTime.Now;
                _context.Stars.Update(star);
                await _context.SaveChangesAsync();
            };

            return new RestDTO<Star?>()
            {
                Data = star,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Stars",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [Authorize(Roles = RoleNames.Administrator)]
        [HttpDelete(Name = "DeleteStar")]
        public async Task<RestDTO<Star?>> Delete(int id)
        {
            var star = await _context.Stars
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (star != null)
            {
                _context.Stars.Remove(star);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Star?>()
            {
                Data = star,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Stars",
                                id,
                                Request.Scheme)!,
                            "self",
                            "DELETE"),
                }
            };
        }
    }
}
