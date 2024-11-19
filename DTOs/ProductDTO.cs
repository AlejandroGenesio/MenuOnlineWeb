using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? SellMinOptions { get; set; }
        public decimal? SellMinPrice { get; set; }
        public List<ProductImageDTO> Images { get; set; } = new List<ProductImageDTO>();
    }
}
