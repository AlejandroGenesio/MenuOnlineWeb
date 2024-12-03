using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public string? Variants { get; set; }
        public string? Modifiers { get; set; }
        public int OrderId { get; set; }
    }
}
