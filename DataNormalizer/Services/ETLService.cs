using DataNormalizer.Data;
using DataNormalizer.Models;
using FileHelpers;
using Microsoft.Data.SqlClient;
using System.Text;

namespace DataNormalizer.Services
{
    public class ETLService
    {
        private readonly AppDbContext _context;


        public ETLService(AppDbContext context)
        {
            _context = context;
        }
        public async void UploadDataToDb(IFormFile file)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var engine = new FileHelperEngine<DatabaseFormDTO>(Encoding.UTF8);
                var records = engine.ReadStream(reader);

                foreach (DatabaseFormDTO item in records)
                {
                    var dataBaseForm = new DatabaseForm(item);
                    _context.databaseForms.Add(dataBaseForm);
                    _context.SaveChanges();
                }
            }
        }
    }
}
