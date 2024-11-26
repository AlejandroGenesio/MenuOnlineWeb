namespace MenuOnlineUdemy.DTOs
{
    public class ProductModifierGroupDTO
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public decimal? ExtraPrice { get; set; }
        public string? OptionsGroup { get; set; }
        public int GroupStyle { get; set; } = 0;
        public int GroupStyleClosed { get; set; } = 0;
    }
}
