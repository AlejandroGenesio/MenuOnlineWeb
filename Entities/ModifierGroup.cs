using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class ModifierGroup
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string? Label { get; set; }
        public decimal? ExtraPrice { get; set; }
        [StringLength(1000)]
        public string? OptionsGroup { get; set; }
        public int GroupStyle { get; set; } = 0;
        public bool GroupStyleClosed { get; set; }
        public List<ModifierGroupModifierOption>? ModifierGroupModifierOptions { get; set; } = new List<ModifierGroupModifierOption>();
    }
}
