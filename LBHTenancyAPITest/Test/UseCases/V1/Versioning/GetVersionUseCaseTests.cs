using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.UseCases.V1.Versioning;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.V1.Versioning
{
    public class GetVersionUseCaseTests
    {
        private readonly IGetVersionUseCase _classUnderTest;
        public GetVersionUseCaseTests()
        {
            _classUnderTest = new GetVersionUseCase();
        }

        [Fact]
        public async Task can_get_version_number_from_assembly()
        {
            //arrange
            //act
            var response = await _classUnderTest.ExecuteAsync(CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Version.Should().NotBeNull();
            response.Version.InformationalVersion.Should().Contain("1.0.0.0");
        } 
    }
}
