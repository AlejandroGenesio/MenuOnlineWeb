using MenuOnlineUdemy.DTOs;
using OfficeOpenXml;
using System.Xml.Serialization;
using static OfficeOpenXml.ExcelErrorValue;

namespace MenuOnlineUdemy.services.Import
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductBulkImportHandler
    {
        /// <summary>
        /// Receives a file and imports several products
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
                    var modifierGroupsWorksheet = package.Workbook.Worksheets[1];
                    var modifierGroupsOptionsWorksheet = package.Workbook.Worksheets[2];
                    var variantWorksheet = package.Workbook.Worksheets[3]; // Variants


                    HandleWorksheet(productWorkshet, importDto, HandleProductRow);
                    HandleWorksheet(modifierGroupsWorksheet, importDto, HandleModifierGroupsWithProductMappingRow, rowOffset: 1);
                    HandleWorksheet(modifierGroupsOptionsWorksheet, importDto, HandleModifierGroupOptions, rowOffset: 1);
                    HandleWorksheet(variantWorksheet, importDto, HandleVariantRow);

                    await importBusinessLogic.Import(importDto);
                }
            }
        }

        private void HandleModifierGroupsWithProductMappingRow(CustomExcelRow row, ProductBulkImportDTO importDto)
        {
            List<string> productNamesOrdered = new();
            const int firstProductColumnOffset = 7; // Excel H column;
            if (row.Index == 1)
            {
                // Header, first row, get product names. The order matters                
                productNamesOrdered = row.GetValuesUntilTheEnd(firstProductColumnOffset);
                importDto.ProductNamesForProductModifierGroupMapping = productNamesOrdered;
                return;
            }


            ImportModifierGroupDTO dto = TransformProductModifierRow(row, firstProductColumnOffset, importDto.ProductNamesForProductModifierGroupMapping);

            if (dto != null)
            {
                importDto.ModifierGroups.Add(dto);
            }
        }

        private void HandleModifierGroupOptions(CustomExcelRow row, ProductBulkImportDTO importDto)
        {
            List<string> productNamesOrdered = new();
            ImportModifierGroupOptionDTO dto = TransformModifierGroupOptionRow(row);

            if (dto != null)
            {
                importDto.ModifierGroupOptions.Add(dto);
            }
        }


        private ImportModifierGroupOptionDTO TransformModifierGroupOptionRow(CustomExcelRow value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            int currentColumn = 0;
            var result = new ImportModifierGroupOptionDTO()
            {
                Id = value.GetIntValue(currentColumn++, 0).GetValueOrDefault(),
                Name = value.GetTextValue(currentColumn++),
                Description = value.GetTextValue(currentColumn++),
                Price = value.GetDecimalValue(currentColumn++),
                GroupOptionName = value.GetTextValue(currentColumn++)
            };
            
            return result;
        }

        private ImportModifierGroupDTO TransformProductModifierRow(CustomExcelRow value, int firstProductColumnOffset, List<string> orderedAvailableProducts)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            int currentColumn = 0;
            var result = new ImportModifierGroupDTO()
            {
                Id = value.GetIntValue(currentColumn++, 0).GetValueOrDefault(),
                OptionsGroup = value.GetTextValue(currentColumn++),
                Label = value.GetTextValue(currentColumn++),
                GroupStyle = GetGroupStyle(value.GetTextValue(currentColumn++)),
                GroupStyleClosed = value.GetBoolValue(currentColumn++, false)!.Value,
                ExtraPrice = value.GetDecimalValue(currentColumn++)                
            };

            // Products are ordered. The ones marked with an 'x' are meant to be included
            result.MappedWithProductNames = orderedAvailableProducts.Where((productName, index) =>
            {
                return string.Equals(value.GetTextValue(firstProductColumnOffset + index)?.Trim(), "x",
                    StringComparison.OrdinalIgnoreCase);
            }).ToHashSet(StringComparer.OrdinalIgnoreCase);


            int GetGroupStyle(string value)
            {
                switch (value)
                {
                    case "CheckBoxes":
                        return 0;
                    case "SingleOption":
                        return 1;
                    case "Dropdown":
                        return 2;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid group style {value}");
                }
            }

            return result;
        }



        private void HandleVariantRow(CustomExcelRow row, ProductBulkImportDTO importDto)
        {
            ImportProductVariantDTO variantDto = TransformVariantRow(row);

            if (variantDto != null)
            {
                importDto.Variants.Add(variantDto);
            }
        }

        private void HandleProductRow(CustomExcelRow row, ProductBulkImportDTO importDto)
        {

            ProductDTO productDto = TransformProductRow(row);

            if (productDto != null)
            {
                importDto.Products.Add(productDto);
            }

        }

        private void HandleWorksheet(ExcelWorksheet worksheet, ProductBulkImportDTO importDto, Action<CustomExcelRow, ProductBulkImportDTO> handleRowMethod, int rowOffset = 2, int columnOffset = 0)
        {
            // Excel indexes start at 1
            // rowOffset = 2 by default, to skip the header. 

            int row = rowOffset;

            for (; row <= worksheet.Dimension.Rows; row++)
            {
                // Access the row using ExcelRangeRow
                ExcelRange rowRange = worksheet.Cells[row, 1, row, worksheet.Dimension.Columns];

                CustomExcelRow rowToTransform = new CustomExcelRow(rowRange, row);

                handleRowMethod(rowToTransform, importDto);

            }
        }

        private static ProductDTO TransformProductRow(CustomExcelRow value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            int currentColumn = 0;
            var result = new ProductDTO()
            {
                Id = value.GetIntValue(currentColumn++, 0).GetValueOrDefault(),
                Name = value.GetTextValue(currentColumn++),
                Description = value.GetTextValue(currentColumn++),
                Price = value.GetDecimalValue(currentColumn++),
                SellMinOptions = value.GetIntValue(currentColumn++),
                SellMinPrice = value.GetDecimalValue(currentColumn++),
            };

            return result;
        }

        private static ImportProductVariantDTO TransformVariantRow(CustomExcelRow value)
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            int currentColumn = 0;
            var result = new ImportProductVariantDTO()
            {
                Id = value.GetIntValue(currentColumn++, 0).GetValueOrDefault(),
                Name = value.GetTextValue(currentColumn++),
                Description = value.GetTextValue(currentColumn++),
                Price = value.GetDecimalValue(currentColumn++),
                Stock = value.GetIntValue(currentColumn++, 0).GetValueOrDefault(),
                ProductName = value.GetTextValue(currentColumn++)
            };

            return result;
        }

        private class CustomExcelRow
        {

            public int Index { get; }

            private readonly ExcelRange innerRange;

            public CustomExcelRow(ExcelRange innerRange, int rowIndex)
            {
                this.innerRange = innerRange;
                Index = rowIndex;
            }

            public string GetTextValue(int column)
            {
                return GetTextValue(innerRange, column);
            }

            private string GetTextValue(ExcelRange innerRange, int column)
            {
                return innerRange.GetCellValue<string>(column);
            }

            public int? GetIntValue(int column, int? defaultValue = null)
            {
                var value = innerRange.GetCellValue<string>(column);
                return int.TryParse(value, out var result) ? result : defaultValue;
            }

            public decimal? GetDecimalValue(int column, decimal? defaultValue = null)
            {
                var value = innerRange.GetCellValue<string>(column);
                return decimal.TryParse(value, out var result) ? result : defaultValue;
            }

            public bool? GetBoolValue(int column, bool? defaultValue = null)
            {
                var value = innerRange.GetCellValue<string>(column);
                return bool.TryParse(value, out var result) ? result : defaultValue;
            }

            public List<string> GetValuesUntilTheEnd(int absoluteColumnOffset)
            {
                List<string> result = new List<string>();
                for (int currentColumn = absoluteColumnOffset; currentColumn < innerRange.Columns; currentColumn++)
                {
                    result.Add(GetTextValue(currentColumn));
                }


                return result;
            }
        }

    }



}
