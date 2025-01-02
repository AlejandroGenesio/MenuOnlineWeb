namespace MenuOnlineUdemy.DTOs
{
    public class ProductBulkImportDTO
    {
        public List<ProductDTO> Products{ get; set; } = new();

        public List<ImportProductVariantDTO> Variants { get; set; } = new();

        public List<ImportModifierGroupDTO> ModifierGroups { get; set; } = new();

        public List<ImportModifierGroupOptionDTO> ModifierGroupOptions { get; set; } = new();

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

    public class ImportModifierGroupOptionDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? GroupOptionName { get; set; }
    }

}
