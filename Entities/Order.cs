using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime timestamp { get; set; } = DateTime.Now;
        public decimal? TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        [StringLength(50)]
        public string? ClientName { get; set; }
    }
} 
