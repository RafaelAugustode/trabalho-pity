using Microsoft.Extensions.Configuration;

namespace DataVault.Data
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionString;

        
        public ConnectionStringProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string GetConnectionString() => _connectionString;
    }
}