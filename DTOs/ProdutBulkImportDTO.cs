namespace MenuOnlineUdemy.DTOs
{
    public class ProductBulkImportDTO
    {
        public List<ProductDTO> Products{ get; set; }

        public List<ImportProductVariantDTO> Variants { get; set; }

    }
}
