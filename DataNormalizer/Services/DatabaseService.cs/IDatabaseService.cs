namespace DataNormalizer.Services.DatabaseService.cs
{
    public interface IDatabaseService
    {
        public List<dynamic> GetDataFromDatabase(string connectionString, string query);
    }
}
