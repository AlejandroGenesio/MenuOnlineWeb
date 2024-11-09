namespace MenuOnlineUdemy.DTOs
{
    public class ImportProductVariantDTO : IVariantDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public string? ProductName { get; set; }
    }
}
