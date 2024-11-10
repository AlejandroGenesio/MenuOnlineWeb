using MenuOnlineUdemy.DTOs;
using OfficeOpenXml;
using System.Xml.Serialization;

namespace MenuOnlineUdemy.services.Import
{
    public interface IProductBulkImportHandler
    {
        bool Import(IFormFile file);
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

        bool IProductBulkImportHandler.Import(IFormFile file)
        {
            throw new NotImplementedException();
        }

        async Task ImportFile(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                // Copy the IFormFile stream to a MemoryStream
                await file.CopyToAsync(stream);
                stream.Position = 0; // Reset the position to the start of the stream

                // Use EPPlus to load the Excel file
                using (var package = new ExcelPackage(stream))
                {
                    var productWorkshet = package.Workbook.Worksheets[0]; // Products
                    var variantWorksheet = package.Workbook.Worksheets[1]; // Variants

                    for (int row = productWorkshet.Dimension.Start.Row; row <= productWorkshet.Dimension.End.Row; row++)
                    {
                        for (int col = productWorkshet.Dimension.Start.Column; col <= productWorkshet.Dimension.End.Column; col++)
                        {
                            var cellValue = productWorkshet.Cells[row, col].Value;
                            Console.Write(cellValue + "\t");
                        }
                        Console.WriteLine();
                    }


                    // TODO: read data from excel


                    // Loop through rows and columns
                    //for (int row = worksheet.Dimension.Start.Row; row <= worksheet.Dimension.End.Row; row++)
                    //{
                    //    for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                    //    {
                    //        var cellValue = worksheet.Cells[row, col].Text;
                    //        Console.Write(cellValue + "\t");
                    //    }
                    //    Console.WriteLine();
                    //}


                    ProductBulkImportDTO importDto = new ProductBulkImportDTO();

                    await importBusinessLogic.Import(importDto);


                }
            }
        }




    }

}
