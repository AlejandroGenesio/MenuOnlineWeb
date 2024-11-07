using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class CreateOrderDTO
    {
        public DateTime timestamp { get; set; } = DateTime.Now;
        public decimal? TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? ClientName { get; set; }

    }
}
