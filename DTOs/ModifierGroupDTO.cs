using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class ModifierGroupDTO
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public decimal? ExtraPrice { get; set; }
        public string? OptionsGroup { get; set; }
        public int GroupStyle { get; set; } = 0;
        public bool GroupStyleClosed { get; set; } = false;
        public List<ModifierGroupModifierOptionDTO> ModifierOptions { get; set; } = new List<ModifierGroupModifierOptionDTO>();
    }
}
