using DataNormalizer.Data;
using MongoDB.Driver;

namespace DataNormalizer.Services
{

    //Need to implemnet thee mongo db instance
    public class MongoDbETLService
    {
        private readonly AppDbContext _appDbContext;
        public MongoDbETLService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void UploadDataToDb(string connectionString, Dictionary<string, dynamic> tableMappings, string databaseName)
        {
            foreach(var table in tableMappings)
            {
                var collection = GetCollection(connectionString,databaseName ,table.Key,table.Value.EntityType.ClrType);
                foreach(var item in collection)
                {
                    
                }
            }
        }

        private object GetCollection(string connectionString, string dataBaseName, string collectionName, Type collectionType)
        {
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(dataBaseName);
            // Use reflection to create the GetCollection<> method
            var getCollectionMethod = typeof(IMongoDatabase).GetMethod("GetCollection").MakeGenericMethod(collectionType);
            var collection = getCollectionMethod.Invoke(database, new object[] { collectionName });

            return collection;

        }
    }
}
