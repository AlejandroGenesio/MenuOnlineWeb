using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? sell_min_price { get; set; }

    }
} 
