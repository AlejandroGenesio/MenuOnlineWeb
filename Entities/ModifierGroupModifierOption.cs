namespace MenuOnlineUdemy.Entities
{
    public class ModifierGroupModifierOption
    {
        public int ModifierOptionId { get; set;}
        public ModifierOption? ModifierOption { get; set;}
        public int ModifierGroupId { get; set; }
        public ModifierGroup? ModifierGroup { get; set;}
    }
}
