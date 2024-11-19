namespace MenuOnlineUdemy.DTOs
{
    public class ProductBulkImportDTO
    {
        public List<ProductDTO> Products{ get; set; } = new();

        public List<ImportProductVariantDTO> Variants { get; set; } = new();

    }
}
