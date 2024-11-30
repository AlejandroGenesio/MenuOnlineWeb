using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class CreateModifierGroupDTO
    {
        public string? Label { get; set; }
        public decimal? ExtraPrice { get; set; }
        public string? OptionsGroup { get; set; }
        public int GroupStyle { get; set; } = 0;
        public bool GroupStyleClosed { get; set; }

    }
}
