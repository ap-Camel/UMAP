//using MySql.Data.MySqlClient;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;

//namespace Umap.Api.Data.Impl
//{
//    public class TransactionService : ITransactionService
//    {

//        private readonly IDbConnectionFactory _connectionFactory;
//        private IDbTransaction _transaction;


//        public TransactionService(IDbConnectionFactory connectionFactory)
//        {
//            _connectionFactory = connectionFactory;
//        }

//        public async Task BeginAsync(CancellationToken cancellationToken = default)
//        {
//            var connection = _connectionFactory.Create<MySqlConnection>();
//            await connection.OpenAsync();

//            await _connection.OpenAsync();
//            _tx = _conn.BeginTransaction();
//        }


//        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
//        {
//            try
//            {
//                using (IDbConnection connection = new MySqlConnection(_ConnectionString))
//                {
//                    await connection.OpenAsync(); // Asynchronously open a connection to the database
//                    return await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
//                }
//            }
//            catch (TimeoutException ex)
//            {
//                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout", GetType().FullName), ex);
//            }
//            catch (SqlException ex)
//            {
//                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
//            }
//        }


//    }
//}
