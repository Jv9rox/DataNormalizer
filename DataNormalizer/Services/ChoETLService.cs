using ChoETL;
using DataNormalizer.Data;
using DataNormalizer.Models;
using DataNormalizer.Services.Helpers;

namespace DataNormalizer.Services
{
    public class ChoETLService
    {
        private readonly AppDbContext _context;

        public ChoETLService(AppDbContext context)
        {
            _context = context;
        }
        public void UploadDataToDb(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            switch (extension.ToLower())
            {
                case ".xlsx":
                    {
                        handleExcelFile(file);
                        break;
                    }
                case ".csv":
                    {
                        handleCsvFile(file);
                        break;
                    }
                case ".xml":
                    {
                        handleXmlFile(file); break;
                    }
                case ".json":
                    {
                        handleJsonFile(file); break;
                    }
                case ".yaml":
                    {
                        handleYamlFile(file); break;
                    }
                case ".parquet":
                    {
                        handleParquetFile(file); break;
                    }
                case ".avro":
                    {
                        handleAvroFile(file); break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void handleCsvFile(IFormFile file)
        {
            using (var reader = new ChoCSVReader<DatabaseFormDTO>(file.OpenReadStream()).WithFirstLineHeader())
            {
                dynamic rec;
                while ((rec = reader.Read()) != null)
                {
                    _context.databaseForms.Add(new DatabaseForm(rec));
                    _context.SaveChanges();
                }
            }
        }
        private void handleExcelFile(IFormFile file)
        {
            handleCsvFile(ExcelToCsvConverter.ConvertToCsv(file));
        }
        private void handleXmlFile(IFormFile file)
        {
            using (var reader = new ChoXmlReader<DatabaseFormDTO>(file.OpenReadStream()))
            {
                dynamic rec;
                while ((rec = reader.Read()) != null)
                {
                    _context.databaseForms.Add(new DatabaseForm(rec));
                    _context.SaveChanges();
                }
            }
        }
        private void handleJsonFile(IFormFile file)
        {
            using (var reader = new ChoJSONReader<DatabaseFormDTO>(file.OpenReadStream()))
            {
                dynamic rec;
                while ((rec = reader.Read()) != null)
                {
                    _context.databaseForms.Add(new DatabaseForm(rec));
                    _context.SaveChanges();
                }
            }
        }
        private void handleYamlFile(IFormFile file)
        {
            using (var reader = new ChoYamlReader<DatabaseFormDTO>(file.OpenReadStream()).WithYamlPath("$.*[*]"))
            {
                dynamic rec;
                while ((rec = reader.Read()) != null)
                {
                    _context.databaseForms.Add(new DatabaseForm(rec));
                    _context.SaveChanges();
                }
            }
        }
        private void handleParquetFile(IFormFile file)
        {
            using (var reader = new ChoParquetReader<DatabaseFormDTO>(file.OpenReadStream()))
            {
                dynamic rec;
                while ((rec = reader.Read()) != null)
                {
                    _context.databaseForms.Add(new DatabaseForm(rec));
                    _context.SaveChanges();
                }
            }
        }
        private void handleAvroFile(IFormFile file)
        {
            using (var reader = new ChoAvroReader<DatabaseFormDTO>(file.OpenReadStream()))
            {
                dynamic rec;
                while ((rec = reader.Read()) != null)
                {
                    _context.databaseForms.Add(new DatabaseForm(rec));
                    _context.SaveChanges();
                }
            }
        }
    }
}
