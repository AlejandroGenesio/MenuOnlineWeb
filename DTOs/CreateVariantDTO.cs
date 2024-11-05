using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class CreateVariantDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal price { get; set; } = 0;
        public int stock { get; set; } = 0;

    }
}
