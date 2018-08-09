using System;
using LBHTenancyAPI.Interfaces;
using LBHTenancyAPI.Services;
using LBHTenancyAPITest.ArrearsAgreementService;
using Xunit;

namespace LBHTenancyAPITest.Test.Services
{
    public interface ArrearsServiceRequestBuilderTests
    {
        [Fact]
        public void Return_A_Built_Request_Object()
        {
            var builder = new ArrearsServiceRequestBuilder(new NameValueCollection());
            var request = builder.BuildArrearsRequest(new ArrearsActionCreateRequest { action = new ArrearsActionInfo() });
            Assert.IsType<ArrearsActionCreateRequest>(request);
        }
    }
}
