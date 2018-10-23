using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LBHTenancyAPI.Factories;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;

namespace LBHTenancyAPI.Infrastructure.V1.Health
{
    public class SqlConnectionHealthCheck:IHealthCheck
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ILogger<SqlConnectionHealthCheck> _logger;

        public SqlConnectionHealthCheck(ISqlConnectionFactory sqlConnectionFactory, ILogger<SqlConnectionHealthCheck> logger)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _logger = logger;
        }
        public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _logger.LogInformation($"SqlConnectionHealthCheck: CheckAsync Started - {stopwatch.ElapsedMilliseconds}ms");
            _logger.LogInformation($"SqlConnectionHealthCheck: Started Creating SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
            using (var sqlConnection = _sqlConnectionFactory.Create())
            {
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Creating SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                _logger.LogInformation($"SqlConnectionHealthCheck: Started Opening SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                try
                {
                    await sqlConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy($"Could not connect to database - {ex.Message} - {stopwatch.ElapsedMilliseconds}ms");
                }
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Opening SqlConnection - {stopwatch.ElapsedMilliseconds}ms");

                _logger.LogInformation($"SqlConnectionHealthCheck: Started Querying tenagree - {stopwatch.ElapsedMilliseconds}ms");
                var result = await sqlConnection.QueryAsync<string>("SELECT TOP 1 tag_ref from tenagree WHERE tenagree.tag_ref IS NOT NULL").ConfigureAwait(false);
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Querying tenagree - {stopwatch.ElapsedMilliseconds}ms");

                var list = result?.ToList();
                if (list == null || !list.Any())
                {
                    _logger.LogInformation($"SqlConnectionHealthCheck: Started Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                    sqlConnection.Close();
                    _logger.LogInformation($"SqlConnectionHealthCheck: Finished Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                    return HealthCheckResult.Unhealthy($"Could not get results from database - {stopwatch.ElapsedMilliseconds}ms");
                }
                
                _logger.LogInformation($"SqlConnectionHealthCheck: Started Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");
                sqlConnection.Close();
                _logger.LogInformation($"SqlConnectionHealthCheck: Finished Closing SqlConnection - {stopwatch.ElapsedMilliseconds}ms");

                _logger.LogInformation($"SqlConnectionHealthCheck: CheckAsync Finished - {stopwatch.ElapsedMilliseconds}ms");
                return list.Count > 0 ?  HealthCheckResult.Healthy($"Successfully retrieved 1 record from database - {stopwatch.ElapsedMilliseconds}ms"): HealthCheckResult.Unhealthy($"Could not get a valid record from database - {stopwatch.ElapsedMilliseconds}ms");
            }

        }
    }
}
