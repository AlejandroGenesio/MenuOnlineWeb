namespace MenuOnlineUdemy.Entities
{
    public class Variant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal price { get; set; } = 0;
        public int stock { get; set; } = 0;
    }
}
