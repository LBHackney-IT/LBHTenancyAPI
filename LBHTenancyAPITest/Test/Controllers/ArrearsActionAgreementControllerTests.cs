using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class ArrearsActionAgreementControllerTests
    {
        private ArrearsAgreementController _classUnderTest;
        private Mock<ICreateArrearsAgreementUseCase> _mock;

        public ArrearsActionAgreementControllerTests()
        {
            _mock = new Mock<ICreateArrearsAgreementUseCase>();
            _classUnderTest = new ArrearsAgreementController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidCreateArrearsAgreementRequest_ShouldReturn200()
        {
            //arrange
            
            var request = new CreateArrearsAgreementRequest();
            //act
            var response = await _classUnderTest.Post(request);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
        }
    }
}
