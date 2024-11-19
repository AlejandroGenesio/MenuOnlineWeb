using MenuOnlineUdemy.DTOs;
using OfficeOpenXml;
using System.Xml.Serialization;

namespace MenuOnlineUdemy.services.Import
{
    public interface IProductBulkImportHandler
    {
        Task<bool> Import(IFormFile file);
    }

    internal class ProductBulkImportHandler : IProductBulkImportHandler
    {

        private readonly ProductBulkImportBusinessLogic importBusinessLogic;

        public ProductBulkImportHandler(ProductBulkImportBusinessLogic importBusinessLogic)
        {
            this.importBusinessLogic = importBusinessLogic;
        }

        public async Task<bool> Import(IFormFile file)
        {

            // Parse file aexcel
            // Convertir
            await ImportFile(file);


            return true;
        }

        async Task ImportFile(IFormFile file)
        {
            const int productsWorksheetIndex = 0;
            using (var stream = new MemoryStream())
            {
                // Copy the IFormFile stream to a MemoryStream
                await file.CopyToAsync(stream);
                stream.Position = 0; // Reset the position to the start of the stream
                
                ProductBulkImportDTO importDto = new ProductBulkImportDTO();

                // Use EPPlus to load the Excel file
                using (var package = new ExcelPackage(stream))
                {

                    var productWorkshet = package.Workbook.Worksheets[productsWorksheetIndex]; // Products
                    var variantWorksheet = package.Workbook.Worksheets[1]; // Variants


                    int row;

                    for (row = 2; row <= productWorkshet.Dimension.Rows; row++)
                    {
                        // Access the row using ExcelRangeRow
                        ExcelRange rowRange = productWorkshet.Cells[row, 1, row, productWorkshet.Dimension.Columns];

                        CustomExcelRow rowToTransform = new CustomExcelRow(rowRange);

                        var productDto = TransformProductRow(rowToTransform);

                        if (productDto == null)
                        {
                            // TODO: add error?
                            continue;
                        }
                        
                        importDto.Products.Add(productDto);
                    }


                    await importBusinessLogic.Import(importDto);
                }
            }
        }



        private static ProductDTO TransformProductRow(CustomExcelRow value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            int currentColumn = 0;
            var result = new ProductDTO()
            {
                Id = value.GetIntValue(currentColumn++, 0).Value,
                Name = value.GetTextValue(currentColumn++),
                Description= value.GetTextValue(currentColumn++),
                Price = value.GetDecimalValue(currentColumn++),
                SellMinOptions = value.GetIntValue(currentColumn++),
                SellMinPrice = value.GetDecimalValue(currentColumn++),
            };

            return result;
        }

        private class CustomExcelRow
        {

            private readonly ExcelRange innerRange;

            public CustomExcelRow(ExcelRange innerRange)
            {
                this.innerRange = innerRange;
            }

            public string GetTextValue(int column)
            {
                return innerRange.GetCellValue<string>(column);
            }

            public int? GetIntValue(int column, int? defaultValue = null)
            {
                var value = innerRange.GetCellValue<string>(column);
                return int.TryParse(value, out var result)? result: defaultValue;
            }

            public decimal? GetDecimalValue(int column, decimal? defaultValue = null)
            {
                var value = innerRange.GetCellValue<string>(column);
                return decimal.TryParse(value, out var result) ? result : defaultValue;
            }
        }

    }



}
