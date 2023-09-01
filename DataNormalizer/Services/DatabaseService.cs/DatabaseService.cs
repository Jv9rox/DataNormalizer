using System.Data.Common;
using System.Data;

namespace DataNormalizer.Services.DatabaseService.cs
{
    public class DatabaseService:IDatabaseService
    {
        public List<dynamic> GetDataFromDatabase(string connectionString, string query)
        {
            // Adjust the provider name and ADO.NET DbProviderFactory based on the database type.
            string providerName = "System.Data.SqlClient"; // Example: SQL Server
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                if (connection == null)
                {
                    throw new Exception("Database provider not found.");
                }

                connection.Open();

                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        var results = new List<dynamic>();

                        while (reader.Read())
                        {
                            var record = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                record[reader.GetName(i)] = reader[i];
                            }

                            results.Add(record);
                        }

                        return results;
                    }
                }
            }
        }
            }
}
