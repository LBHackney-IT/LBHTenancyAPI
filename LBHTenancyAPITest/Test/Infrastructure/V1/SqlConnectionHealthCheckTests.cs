using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Factories;
using LBHTenancyAPI.Infrastructure.V1.Health;
using LBHTenancyAPITest.Helpers;
using LBHTenancyAPITest.Helpers.Data;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LBHTenancyAPITest.Test.Infrastructure.V1
{
    public class SqlConnectionHealthCheckTests : IClassFixture<DatabaseFixture>
    {
        private IHealthCheck _classUnderTest;
        private SqlConnectionFactory _sqlConnectionFactory;
        private SqlConnection _db;
        public SqlConnectionHealthCheckTests(DatabaseFixture databaseFixture)
        {
            _db = databaseFixture.Db;
            var connection = DotNetEnv.Env.GetString("UH_CONNECTION_STRING");
            var loggerFactory = new LoggerFactory();
            _sqlConnectionFactory = new SqlConnectionFactory(connection, loggerFactory.CreateLogger<SqlConnectionFactory>());
            var logger = loggerFactory.CreateLogger<SqlConnectionHealthCheck>();
            _classUnderTest = new SqlConnectionHealthCheck(_sqlConnectionFactory, logger);
        }

        [Fact]
        public async Task can_connect_to_sql_database_returns_health_status()
        {
            //arrange
            var fakeTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            TestDataHelper.InsertTenancy(fakeTenancy,_db);
            //act
            var result = await _classUnderTest.CheckAsync(CancellationToken.None).ConfigureAwait(false);
            //assert
            result.Should().NotBeNull();
            result.CheckStatus.Should().Be(CheckStatus.Healthy);
        }

        [Fact]
        public async Task cannot_connect_to_sql_database_and_return_record_returns_unhealthy_status()
        {
            //arrange
            var connection = DotNetEnv.Env.GetString("test");
            var loggerFactory = new LoggerFactory();
            _sqlConnectionFactory = new SqlConnectionFactory(connection, loggerFactory.CreateLogger<SqlConnectionFactory>());
            var logger = loggerFactory.CreateLogger<SqlConnectionHealthCheck>();
            _classUnderTest = new SqlConnectionHealthCheck(_sqlConnectionFactory, logger);
            //act
            var result = await _classUnderTest.CheckAsync(CancellationToken.None).ConfigureAwait(false);
            //assert
            result.Should().NotBeNull();
            result.CheckStatus.Should().Be(CheckStatus.Unhealthy);
        }
    }
}
