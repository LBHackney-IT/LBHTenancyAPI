using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.HealthChecks;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V1
{
    public class HealthcheckControllerTests
    {
        private Mock<IHealthCheckService> _mockHealthCheckService;
        private HealthcheckController _classUnderTest;

        public HealthcheckControllerTests()
        {
            _mockHealthCheckService = new Mock<IHealthCheckService>();
            _classUnderTest = new HealthcheckController(_mockHealthCheckService.Object);
        }

        [Fact]
        public void WhenCalled_Healthcheck_ShouldIncludeSuccessContent()
        {
            var result = _classUnderTest.Healthcheck() as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(new Dictionary<string, object> {{"success", true}}, result.Value);
        }

        [Fact]
        public void WhenCalled_Healthcheck_ShouldRespond()
        {
            var result = _classUnderTest.Healthcheck() as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task WhenCalled_DetailedHealthcheck_ShouldIncludeHealthHealth()
        {
            var result = await _classUnderTest.StatusHealthCheck().ConfigureAwait(false) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task WhenCalled_DetailedHealthcheck_ShouldRespond_WithHealth()
        {
            //arrange
            _mockHealthCheckService.Setup(s => s.CheckHealthAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CompositeHealthCheckResult(CheckStatus.Healthy));
            //act
            var result = await _classUnderTest.StatusHealthCheck().ConfigureAwait(false) as OkObjectResult;
            Assert.NotNull(result);
            var healthCheckResult = result.Value as CompositeHealthCheckResult;
            Assert.Equal(healthCheckResult.CheckStatus, new CompositeHealthCheckResult(CheckStatus.Healthy).CheckStatus);
        }

        [Fact]
        public async Task WhenCalled_DetailedHealthcheck_IfCantConnect_ShouldResponseWithUnhealthy()
        {
            //arrange
            _mockHealthCheckService.Setup(s => s.CheckHealthAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CompositeHealthCheckResult(CheckStatus.Unhealthy));
            //act
            var result = await _classUnderTest.StatusHealthCheck().ConfigureAwait(false) as OkObjectResult;
            Assert.NotNull(result);
            var healthCheckResult = result.Value as CompositeHealthCheckResult;
            Assert.Equal(healthCheckResult.CheckStatus, new CompositeHealthCheckResult(CheckStatus.Unhealthy).CheckStatus);
        }
    }
}
