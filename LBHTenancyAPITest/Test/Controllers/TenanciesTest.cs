using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class TenanciesTest
    {
        [Fact]
        public async Task IndexReturnsSuccess()
        {
            var controller = new TenanciesController();
            var result = await controller.Get();
            var response = result as OkObjectResult;

            var expectedResult = new Dictionary<string, string>() {{"foo", "bar"}};
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var json = JsonConvert.SerializeObject(response.Value);

            Assert.NotNull(response);
            Assert.Equal(expectedJson, json);
        }
    }
}
