using System.Data;
using System.Data.Common;

namespace Umap.Api.Data
{
    public interface IDbConnectionFactory
    {
        T Create<T>() where T : DbConnection, new();
    }
}
