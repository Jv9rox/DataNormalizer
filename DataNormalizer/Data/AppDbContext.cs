using DataNormalizer.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataNormalizer.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DatabaseForm> databaseForms { get; set; }
        public DbSet<UploadedFile> uploadedFiles { get; set; }
    }
}
