using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;

namespace LBHTenancyAPI.Infrastructure.Health
{
    public class SqlConnectionHealthCheck:IHealthCheck
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlConnectionHealthCheck> _logger;

        public SqlConnectionHealthCheck(string connectionString, ILogger<SqlConnectionHealthCheck> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }
        public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _logger.LogInformation($"SqlConnectionHealthCheck: CheckAsync Started - {stopwatch.ElapsedMilliseconds}ms");
            _logger.LogInformation($"SqlConnectionHealthCheck: Started Creating SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Creating SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                _logger.LogInformation($"SqlConnectionHealthCheck: Started Opening SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                await sqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Opening SqlConnection - {stopwatch.ElapsedMilliseconds}ms");

                _logger.LogInformation($"SqlConnectionHealthCheck: Started Querying tenagree - {stopwatch.ElapsedMilliseconds}ms");
                var result = await sqlConnection.QueryAsync<string>("SELECT TOP 1 tag_ref from tenagree WHERE tenagree.tag_ref IS NOT NULL").ConfigureAwait(false);
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Querying tenagree - {stopwatch.ElapsedMilliseconds}ms");
                if (result == null)
                {
                    _logger.LogInformation($"SqlConnectionHealthCheck: Started Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                    sqlConnection.Close();
                    _logger.LogInformation($"SqlConnectionHealthCheck: Finished Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                    return HealthCheckResult.Unhealthy($"Could not get results from database - {stopwatch.ElapsedMilliseconds}ms");
                }
                    
                var list = result.ToList();
                _logger.LogInformation($"SqlConnectionHealthCheck: Started Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                sqlConnection.Close();
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");

                _logger.LogInformation($"SqlConnectionHealthCheck: CheckAsync Finished - {stopwatch.ElapsedMilliseconds}ms");
                return list.Count == 0 ? HealthCheckResult.Unhealthy($"Could not get a valid record from database - {stopwatch.ElapsedMilliseconds}ms") : HealthCheckResult.Healthy($"Successfully retrieved 1 record from database - {stopwatch.ElapsedMilliseconds}ms");
            }

        }
    }
}
