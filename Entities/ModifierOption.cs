using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class ModifierOption
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public int enabled {  get; set; } = 1;
        public List<ModifierGroupModifierOption>? ModifierGroupModifierOptions { get; set; } = new List<ModifierGroupModifierOption>();

    }
}
