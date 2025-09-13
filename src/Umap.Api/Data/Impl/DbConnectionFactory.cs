using MySql.Data.MySqlClient;
using System.Data.Common;
using Umap.Api.Data.Settings;

namespace Umap.Api.Data.Impl
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IDatabaseSettings databaseSettings)
        {
            _connectionString = databaseSettings.ConnectionString;
        }

        public T Create<T>() where T : DbConnection, new()
        {
            T connection = new();
            connection.ConnectionString = _connectionString;
            return connection;
        }
    }
}
