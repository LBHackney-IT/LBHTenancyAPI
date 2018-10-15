using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace LBHTenancyAPI.Factories
{
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
        /// Creates sql connection
        /// </summary>
        /// <returns></returns>
        public SqlConnection Create()
        {
            _logger.LogInformation("Creating SqlConnection");
            return new SqlConnection(_connectionString);
        }
    }
}
