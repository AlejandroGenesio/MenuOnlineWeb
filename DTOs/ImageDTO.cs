using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? File { get; set; }
    }
}
