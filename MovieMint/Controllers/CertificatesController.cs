using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMint.DTO;
using MovieMint.Models;
using System.Linq.Dynamic.Core;

namespace MovieMint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CertificatesController : Controller

    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<CertificatesController> _logger;

        public CertificatesController(
            ApplicationDbContext context,
            ILogger<CertificatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetCertificates")]
        public async Task<RestDTO<Certificate[]>> Get(
            [FromQuery] RequestDTO<CertificateDTO> input)
        {
            var query = _context.Certificates.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(b => b.Name.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}").Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            return new RestDTO<Certificate[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Certificates",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateCertificate")]
        public async Task<RestDTO<Certificate?>> Post(CertificateDTO model)
        {
            var certificate = await _context.Certificates
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();

            if (certificate != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    certificate.Name = model.Name;
                    certificate.LastUpdatedDate = DateTime.Now;
                    _context.Certificates.Update(certificate);
                    await _context.SaveChangesAsync();
            };
            return new RestDTO<Certificate?>()
            {
                Data = certificate,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Certificates",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [HttpDelete(Name = "DeleteCertificate")]
        public async Task<RestDTO<Certificate?>> Delete(int id)
        {
            var certificate = await _context.Certificates
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Certificate?>()
            {
                Data = certificate,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Certificates",
                                id,
                                Request.Scheme)!,
                            "self",
                            "DELETE"),
                }
            };

        }

    }
}
