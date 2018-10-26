using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace LBHTenancyAPI.Factories
{
    /// <summary>
    /// Simple Factory for creating SQL connections 
    /// </summary>
    public class SqlConnectionFactory: ISqlConnectionFactory
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlConnectionFactory> _logger;

        public SqlConnectionFactory(string connectionString, ILogger<SqlConnectionFactory> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Creates sql connection however doesn't open it
        /// </summary>
        /// <returns></returns>
        public SqlConnection Create()
        {
            _logger.LogInformation("Creating SqlConnection");
            return new SqlConnection(_connectionString);
        }
    }
}
