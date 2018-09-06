using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Xunit;
using Moq;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class ArrearsActionAgreementControllerTests
    {
        private ArrearsAgreementController _classUnderTest;

        public ArrearsActionAgreementControllerTests()
        {
            var mock = new Mock<ICreateArrearsAgreementUseCase>();
            _classUnderTest = new ArrearsAgreementController(mock.Object);
        }

        [Fact]
        public void GivenValidRequest_ShouldReturn200()
        {
            //arrange
            var request = new CreateArrearsAgreementRequest();
            //act
            var response = _classUnderTest.Post(request);
            //assert
            response.Should().NotBeNull();
        }
    }
}
