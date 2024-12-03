using Microsoft.Extensions.Diagnostics.HealthChecks;
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
        public string? NotesToPrint { get; set; } // Product - Modifiers - Variants - Prices - etc.
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
} 
