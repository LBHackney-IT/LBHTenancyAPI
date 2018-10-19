using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.Versioning;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.Versioning
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
            response.Version.Should().BeEquivalentTo("1.0.0.0");
        } 
    }
}
