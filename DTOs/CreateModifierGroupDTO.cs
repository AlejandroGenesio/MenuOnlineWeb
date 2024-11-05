using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class CreateModifierGroupDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int GroupId { get; set; } = 0;
        public bool IsMandatory { get; set; } = false;
        public int MinToSelect { get; set; } = 0;
        public decimal MinPriceToBuy { get; set; } = 0;
        public int GroupStyle { get; set; } = 0;
        public int GroupStyleClosed { get; set; } = 0;

    }
}
