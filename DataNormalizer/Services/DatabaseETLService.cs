using ChoETL;
using DataNormalizer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DataNormalizer.Services
{
    public class DatabaseETLService
    {
        private readonly AppDbContext _appDbContext;
        public DatabaseETLService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public  T CreateEntityInstance<T>(AppDbContext dbContext) where T : class, new()
        {
            var entityEntry = dbContext.Entry(new T());
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            return entityEntry.Entity;
        }
        public void UploadDataToDb(string connectionString, Dictionary<string, dynamic> tableMappings)
        {
            using var userConnection = new SqlConnection(connectionString);
            userConnection.Open();
            foreach (var table in tableMappings)
            {
                string userQuery = $"SELECT * FROM {table.Key};";
                using var userCommand = new SqlCommand(userQuery, userConnection);
                using var userDataAdapter = new SqlDataAdapter(userCommand);
                var userDataTable = new DataTable();
                userDataAdapter.Fill(userDataTable);
                AddDataTableToDb(userDataTable, table.Key, table.Value);
            }
        }

        private void AddDataTableToDb(DataTable dataTable, string tableName,dynamic dbSet)
        {
            IEntityType entityType = _appDbContext.Model.GetEntityTypes().Where(e => e.GetTableName() == tableName).FirstOrDefault();
            foreach (DataRow row in dataTable.Rows)
            {
                var entity = MapDataRowToEntity(entityType,row);
                // Use reflection to invoke the Add method dynamically
                var addMethod = typeof(AppDbContext).GetMethod("Add", new[] { entityType.ClrType });
                addMethod.Invoke(_appDbContext, new object[] { entity });
                _appDbContext.SaveChanges();
            }
        }
        public object MapDataRowToEntity(IEntityType entityType, DataRow row)
        {
            
            var entity = entityType.ClrType.GetMethod("CreateInstance").Invoke(null, null);
            var primaryKey = entityType.FindPrimaryKey().Properties.FirstOrDefault().Name;
            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property = entityType.ClrType.GetProperty(column.ColumnName);
                if (column.ColumnName != primaryKey) // Skip the primary key column
                {

                    if (property != null && row[column] != DBNull.Value)
                    {
                        property.SetValue(entity, Convert.ChangeType(row[column], property.PropertyType));
                    }
                }
                else
                {
                    property.SetValue(entity, null);
                }
            }
            return entity;
        }

        }
}
