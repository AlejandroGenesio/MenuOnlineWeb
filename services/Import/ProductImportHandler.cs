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
                    var productWorkshet = package.Workbook.Worksheets[0]; // Get the first worksheet
                    var variantWorksheet = package.Workbook.Worksheets[1]; // Get the first worksheet


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

                   await  importBusinessLogic.Import(importDto);


                }
            }
        }


        

    }

}
