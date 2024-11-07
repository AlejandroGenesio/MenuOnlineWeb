using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime timestamp { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? ClientName { get; set; }
    }
}
