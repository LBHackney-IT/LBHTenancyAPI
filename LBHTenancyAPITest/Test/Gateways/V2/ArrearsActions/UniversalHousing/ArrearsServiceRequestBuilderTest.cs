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

        [Theory]
        [InlineData("Test", 500.25, "TST", "dWilliams", "Test diary record 1", "HA1", "PJOAOJEWRT43553454OJ4OJ3", "000017/01")]
        [InlineData("OtherTest", 123, "GEN", "hSmith", "Test diary record 2", "BUC", "PJOAOJEWRT4355345DRE4FJ3", "000017/02")]
        public void builds_a_valid_request_when_valid_parameters_are_provided(string actionCategory, decimal actionBalance, string actionCode,
            string username, string comment, string companyCode, string sessionToken, string tenancyAgreementRef)
        {
            var fakeCredentialsService = new Mock<ICredentialsService>();
            fakeCredentialsService.Setup(s => s.GetUhUserCredentials()).Returns(new UserCredential
            {
                UserName = "testUser",
                UserPassword = "testPassword"
            });
            fakeCredentialsService.Setup(s => s.GetUhSourceSystem()).Returns("TestSystem");
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);
            var serviceRequest = new ActionDiaryRequest
            {
                ActionCategory = actionCategory,
                ActionBalance = actionBalance,
                ActionCode = actionCode,
                Username = username,
                Comment = comment,
                CompanyCode = companyCode,
                SessionToken = sessionToken,
                TenancyAgreementRef = tenancyAgreementRef
            };
            var request = serviceRequestBuilder.BuildNewActionDiaryRequest(serviceRequest);
            Assert.Equal(actionCategory, request.ArrearsAction.ActionCategory);
            Assert.Equal(actionBalance, request.ArrearsAction.ActionBalance);
            Assert.Equal(actionCode, request.ArrearsAction.ActionCode);
            Assert.Equal(comment, request.ArrearsAction.Comment);
            Assert.Equal(tenancyAgreementRef, request.ArrearsAction.TenancyAgreementRef);
            Assert.Equal("testUser", request.DirectUser.UserName);
            Assert.Equal("testPassword", request.DirectUser.UserPassword);
            Assert.Equal("TestSystem", request.SourceSystem);
            Assert.Equal(sessionToken, request.SessionToken);
            Assert.Equal(companyCode, request.CompanyCode);
            Assert.NotEqual(username, request.DirectUser.UserName);

        }
    }
}
