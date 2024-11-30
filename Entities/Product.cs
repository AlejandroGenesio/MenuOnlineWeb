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
        public int? SellMinOptions { get; set; }
        public decimal? SellMinPrice { get; set; }
        public List<Variant> Variants { get; set; } = new List<Variant>();
        public List<ProductImage>? ProductImages { get; set; }

        public List<ProductModifierGroup>? ProductModifierGroups { get; set; } = new List<ProductModifierGroup>();

        public List<ProductCategory>? ProductCategories { get; set; } = new List<ProductCategory>();

    }
} 
