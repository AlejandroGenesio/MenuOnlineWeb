namespace MenuOnlineUdemy.DTOs
{
    public class ProductBulkImportDTO
    {
        public List<ProductDTO> Products{ get; set; } = new();

        public List<ImportProductVariantDTO> Variants { get; set; } = new();

        public List<ImportModifierGroupDTO> ModifierGroups { get; set; } = new();

        public List<string> ProductNamesForProductModifierGroupMapping { get; set; }
    }

    public class ImportModifierGroupDTO
    {
        public int Id { get; set; }
        public string? Label { get; set; }        
        public decimal? ExtraPrice { get; set; }
        public string? OptionsGroup { get; set; }
        public int GroupStyle { get; set; } = 0;
        public bool GroupStyleClosed { get; set; }

        public HashSet<string> MappedWithProductNames { get; set; } = new ();

    }

}
