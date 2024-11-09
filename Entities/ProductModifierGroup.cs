namespace MenuOnlineUdemy.Entities
{
    public class ProductModifierGroup
    {
        public int ModifierGroupId { get; set;}
        public ModifierGroup? ModifierGroup { get; set;}
        public int ProductId { get; set; }
        public Product? Product {get; set;}
        public string? Note { get; set;}
    }
}
