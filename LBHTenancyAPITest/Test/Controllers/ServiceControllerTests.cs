using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class ServiceControllerTests
    {
        private ServiceController _classUnderTest;
        private Mock<IGetServiceDetailsUseCase> _mockServiceDetailsUseCase;

        public ServiceControllerTests()
        {
            _mockServiceDetailsUseCase = new Mock<IGetServiceDetailsUseCase>();
            _classUnderTest = new ServiceController(_mockServiceDetailsUseCase.Object);
        }

        [Fact]
        public async Task GivenGetRequest_ThenServiceController_ShouldReturn200()
        {
            //arrange
            //act
            var response = await _classUnderTest.Get().ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            objectResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GivenGetRequest_ThenServiceController_ShouldReturnServiceDetails()
        {
            //arrange
            _mockServiceDetailsUseCase.Setup(s => s.ExecuteAsync(CancellationToken.None)).ReturnsAsync(
                new GetServiceDetailsResponse
                {
                    ServiceDetails = new ServiceDetails
                    {
                        Name = "LBHTenancyAPI",
                        Description = "Managing Tenancies at Hackney",
                        Organisation = "Hackney Council",
                        Version = new ServiceDetailVersion
                        {
                            Version = "1.0.0.0"
                        }
                    }
                });
            //act
            var response = await _classUnderTest.Get().ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var apiResponse = objectResult.Value as APIResponse<GetServiceDetailsResponse>;
            apiResponse.Should().NotBeNull();
            apiResponse.Data.ServiceDetails.Should().NotBeNull();
            apiResponse.Data.ServiceDetails.Name.Should().Be("LBHTenancyAPI");
            apiResponse.Data.ServiceDetails.Description.Should().Be("Managing Tenancies at Hackney");
            apiResponse.Data.ServiceDetails.Organisation.Should().Be("Hackney Council");
        }

        [Fact]
        public async Task GivenGetRequest_ThenServiceController_ShouldReturn_VersionInfo()
        {
            //arrange
            _mockServiceDetailsUseCase.Setup(s => s.ExecuteAsync(CancellationToken.None)).ReturnsAsync(
                new GetServiceDetailsResponse
                {
                    ServiceDetails = new ServiceDetails
                    {
                        Name = "LBHTenancyAPI",
                        Description = "Managing Tenancies at Hackney",
                        Organisation = "Hackney Council",
                        Version = new ServiceDetailVersion
                        {
                            Version = "1.0.0.0"
                        }
                    }
                });
            //act
            var response = await _classUnderTest.Get().ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var apiResponse = objectResult.Value as APIResponse<GetServiceDetailsResponse>;
            apiResponse.Should().NotBeNull();
            apiResponse.Data.ServiceDetails.Should().NotBeNull();
            apiResponse.Data.ServiceDetails.Version.Version.Should().Be("1.0.0.0");
        }
    }
}
