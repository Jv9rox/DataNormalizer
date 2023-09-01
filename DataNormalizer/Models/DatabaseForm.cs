using FileHelpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;

namespace DataNormalizer.Models
{
    public class DatabaseForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }

        public DatabaseForm() { }
        public DatabaseForm(DatabaseFormDTO databaseFormDTO) 
        {
            ConnectionName = databaseFormDTO.ConnectionName;
            ConnectionString = databaseFormDTO.ConnectionString;
        }
        public static DatabaseForm CreateInstance()
        {
            var instance = new DatabaseForm();
            return instance;
        }
    }
}
