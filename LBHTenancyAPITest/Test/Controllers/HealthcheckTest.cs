using System.Collections.Generic;
using LBHTenancyAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class HealthcheckTest
    {
        public HealthcheckTest()
        {
            var controller = new HealthcheckController();
            result = controller.Healthcheck() as OkObjectResult;
        }

        private readonly OkObjectResult result;

        [Fact]
        public void WhenCalled_Healthcheck_ShouldIncludeSuccessContent()
        {
            Assert.NotNull(result);
            Assert.Equal(new Dictionary<string, object> {{"success", true}}, result.Value);
        }

        [Fact]
        public void WhenCalled_Healthcheck_ShouldRespond()
        {
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
