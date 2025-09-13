namespace Umap.Api.Data.Settings
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; }
        bool LogQueries { get; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public bool LogQueries { get; set; }
    }
}
