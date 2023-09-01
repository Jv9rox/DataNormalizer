using Microsoft.EntityFrameworkCore;

namespace DataNormalizer.Services.Helpers
{
    public class DbSetInstance
    {
        public static dynamic GetDbSetByTableName(DbContext context, string tableName)
        {
            var dbSetProperty = context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                     p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                     p.Name == tableName);

            if (dbSetProperty != null)
            {
                return dbSetProperty.GetValue(context);
            }

            return null;
        }
    }
}
