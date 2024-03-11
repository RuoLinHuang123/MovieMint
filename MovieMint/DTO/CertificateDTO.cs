using System.ComponentModel.DataAnnotations;

namespace MovieMint.DTO
{
    public class CertificateDTO
    {
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
