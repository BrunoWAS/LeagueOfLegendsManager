using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace ApiLeague.Data
{
    public class Database
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
