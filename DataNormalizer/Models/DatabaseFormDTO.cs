using FileHelpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataNormalizer.Models
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class DatabaseFormDTO
    {
            public string id { get; set; }
            public string ConnectionName { get; set; }
            public string ConnectionString { get; set; }
        
    }
}
