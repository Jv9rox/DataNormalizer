using Microsoft.Data.SqlClient;
using Paillave.Etl.Core;
using System;
using Paillave.Etl.FileSystem;
using Paillave.Etl.Zip;
using Paillave.Etl.TextFile;
using Paillave.Etl.SqlServer;
using DataNormalizer.Models;
using System.ComponentModel;

namespace DataNormalizer.Services
{
    public class ETLNETService
    {
        private readonly IConfiguration _configuration;

        public ETLNETService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async void UploadDataToDb(string appfolder)
        {
            var processRunner = StreamProcessRunner.Create<string>(DefineProcess);
            using (var cnx = new SqlConnection())
            {
                cnx.Open();
                var executionOptions = new ExecutionOptions<string>
                {
                    Resolver = new SimpleDependencyResolver().Register(cnx)
                };
                var res = await processRunner.ExecuteAsync(appfolder, executionOptions);
                Console.Write(res.Failed ? "Failed" : "Succeeded");
            }
        }
        private static void DefineProcess(ISingleStream<string> contextStream)
        {
            contextStream
                .CrossApplyFolderFiles("list all required files", "*.zip", true)
                .CrossApplyZipFiles("extract files from zip", "*.csv")
                .CrossApplyTextFile("parse file",
                    FlatFileDefinition.Create(i => new DatabaseFormDTO
                    {
                        ConnectionName = i.ToColumn("ConnectionName"),
                        ConnectionString = i.ToColumn("ConnectionString"),
                    }).IsColumnSeparated(','))
                .Distinct("exclude duplicates", i => i.ConnectionString)
                .SqlServerSave("save in DB", o => o
                    .ToTable("dbo.Person")
                    .SeekOn(p => p.ConnectionString)
                    .DoNotSave(p => p.id))
                .Do("display ids on console", i => Console.WriteLine(i.id));
        }
    }
}
