using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataNormalizer.Models
{
    public class UploadedFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public static UploadedFile CreateInstance()
        {
            var instance = new UploadedFile();
            return instance;
        }
    }
}
