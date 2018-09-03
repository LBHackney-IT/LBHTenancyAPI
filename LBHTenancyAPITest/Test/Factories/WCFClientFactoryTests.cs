using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Factories;
using Xunit;
using FluentAssertions;

namespace LBHTenancyAPITest.Test.Factories
{
    public class WCFClientFactoryTests
    {
        private IWCFClientFactory _classUnderTest;

        [Fact]
        public void GivenInValidDetails_ShouldThrowUriFormatException()
        {
            //arrange
            _classUnderTest = new WCFClientFactory();
            //act
            //assert
            Assert.Throws<UriFormatException>(()=> _classUnderTest.CreateClient<IArrearsAgreementService>("test"));
        }

        [Fact]
        public void GivenValidDetails_ShouldReturnNewClient()
        {
            //arrange
            _classUnderTest = new WCFClientFactory();
            //act
            var client = _classUnderTest.CreateClient<IArrearsAgreementService>("http://www.test.com/test.svc");
            //assert
            client.Should().NotBeNull();
        }
    }
}
