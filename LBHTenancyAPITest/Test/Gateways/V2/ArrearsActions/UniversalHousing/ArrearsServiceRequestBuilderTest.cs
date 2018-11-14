using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using AgreementService;
using LBHTenancyAPI.Gateways.V1.Arrears.UniversalHousing;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Gateways.V2.Arrears.Impl;
using LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing.Impl;
using LBHTenancyAPI.Services.V2;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.V2.ArrearsActions.UniversalHousing
{
    public class ArrearsServiceRequestBuilderTest
    {
        [Fact]
        public void return_a_built_request_object()
        {
            var fakeCredentialsService = new Mock<ICredentialsService>();
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);
            var serviceRequest = new ActionDiaryRequest
            {
                ActionCategory = "test",
                ActionBalance = 0,
                ActionCode = "test",
                Username = "test",
                Comment = "test",
                CompanyCode = "test",
                SessionToken = "test",
                TenancyAgreementRef = "test"
            };
            var request = serviceRequestBuilder.BuildNewActionDiaryRequest(serviceRequest);
            Assert.IsType<ArrearsActionCreateRequest>(request);
        }
    }
}
