using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class CreateImageDTO
    {
        public string? Description { get; set; }
        public IFormFile? File { get; set; }
    }
}
