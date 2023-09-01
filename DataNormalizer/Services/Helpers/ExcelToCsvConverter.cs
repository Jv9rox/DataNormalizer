using System.Text;
using OfficeOpenXml;

namespace DataNormalizer.Services.Helpers
{
    public static class ExcelToCsvConverter
    {
        public static IFormFile ConvertToCsv(IFormFile excelFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                excelFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    var csvContent = new StringBuilder();
                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (col > 1)
                                csvContent.Append(',');

                            csvContent.Append(worksheet.Cells[row, col].Text);
                        }
                        csvContent.AppendLine();
                    }

                    // Convert CSV content to byte array
                    var csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString());

                    // Create an IFormFile instance with CSV content
                    var csvStream = new MemoryStream(csvBytes);
                    var csvFile = new FormFile(csvStream, 0, csvStream.Length, null, Path.GetFileNameWithoutExtension(excelFile.FileName) + ".csv");

                    return csvFile;
                }
            }
        }
    }
}
