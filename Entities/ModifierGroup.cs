using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class ModifierGroup
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        public bool IsMandatory { get; set; } = false;
        public int MinToSelect { get; set; } = 0;
        public decimal MinPriceToBuy { get; set; } = 0;
        public int GroupStyle { get; set; } = 0;
        public int GroupStyleClosed { get; set; } = 0;
        public List<ProductModifierGroup> ProductModifierGroups { get; set; } = new List<ProductModifierGroup>();
    }
}
