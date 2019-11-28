using AgreementService;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing;
using LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing.Impl;
using LBHTenancyAPI.UseCases.V2.ArrearsActions;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;
using Moq;
using System;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V1;
using LBHTenancyAPITest.Helpers;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.V2.Arrears
{
    public class CreateArrearsActionDiaryUseCaseTests
    {
        private ICreateArrearsActionDiaryUseCase _classUnderTest;
        private StubTenanciesGateway _stubTenanciesGateway;
        private Mock<IArrearsActionDiaryGateway> _fakeGateway;

        public CreateArrearsActionDiaryUseCaseTests()
        {
            _fakeGateway = new Mock<IArrearsActionDiaryGateway>();
            _stubTenanciesGateway = new StubTenanciesGateway();
            var credentialsService = new Mock<ICredentialsService>();
            credentialsService.Setup(s => s.GetUhSourceSystem()).Returns("TestSystem");
            credentialsService.Setup(s => s.GetUhUserCredentials()).Returns(new UserCredential
            {
                UserName = "TestUseName",
                UserPassword = "TestUserPassword",
            });
            IArrearsServiceRequestBuilder requestBuilder = new ArrearsServiceRequestBuilder(credentialsService.Object);
            _classUnderTest = new CreateArrearsActionDiaryUseCase(_fakeGateway.Object, _stubTenanciesGateway, requestBuilder);
        }

        [Fact]
        public async Task GivenValidedInput_GatewayReceivesCorrectInput()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            DateTime date = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null);
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(
                    It.Is<ActionDiaryRequest>(i => i.TenancyAgreementRef.Equals("Test"))))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = true,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        ActionCategory = "Test",
                        ActionCode = "HAC",
                        IsCommentOnly = true,
                        UserName = "Test User",
                        Id = 1,
                        TenancyAgreementRef = tenancyAgreementRef
                    }

                });

            var request = new ActionDiaryRequest
            {
                ActionCategory = "Test",
                ActionCode = "HAC",
                Username = "Test User",
                TenancyAgreementRef = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request);
            //assert
            _fakeGateway.Verify(v=> v.CreateActionDiaryEntryAsync(
                It.Is<ActionDiaryRequest>(i => i.TenancyAgreementRef.Equals("Test"))));
            _fakeGateway.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GivenNoUsername_GatewayResponseWith_Success()
        {
            //arrange
            var tenancyAgreementRef = "Test";
            DateTime date = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null);
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(
                    It.Is<ActionDiaryRequest>(i => i.TenancyAgreementRef.Equals("Test"))))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = true,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        ActionCategory = "Test",
                        ActionCode = "HAC",
                        IsCommentOnly = true,
                        UserName = "Default User",
                        Id = 1,
                        TenancyAgreementRef = tenancyAgreementRef
                    }

                });
            var request = new ActionDiaryRequest
            {
                ActionCategory = "Test",
                ActionCode = "HAC",
                Username = null,
                TenancyAgreementRef = tenancyAgreementRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request);
            //assert
            _fakeGateway.Verify(v=> v.CreateActionDiaryEntryAsync(It.Is<ActionDiaryRequest>(i => i.TenancyAgreementRef.Equals("Test"))));
            _fakeGateway.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GivenValidInput_GatewayResponseWith_Success()
        {
            //arrange
            Tenancy tenancy = Fake.GenerateTenancyDetails();
            _stubTenanciesGateway.SetTenancyDetails(tenancy.TenancyRef, tenancy);
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ActionDiaryRequest>(i =>
                    i.TenancyAgreementRef.Equals(tenancy.TenancyRef)
                    )))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = true,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        ActionBalance = tenancy.CurrentBalance,
                        ActionCategory = "Test",
                        ActionCode = "HAC",
                        IsCommentOnly = true,
                        UserName = "Test User",
                        Id = 1,
                        TenancyAgreementRef = tenancy.TenancyRef
                    }
                });
            var request = new ActionDiaryRequest
            {
                ActionCategory = "Test",
                ActionCode = "HAC",
                Username = "Test User",
                TenancyAgreementRef = tenancy.TenancyRef
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request);
            //assert
            Assert.True(response.Success);
            Assert.Equal(tenancy.TenancyRef, response.ArrearsAction.TenancyAgreementRef);
            Assert.Equal(tenancy.CurrentBalance, response.ArrearsAction.ActionBalance);
        }

        [Fact]
        public async Task GivenInvalidInput_GatewayResponseWith_Failure()
        {
            //arrange
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ActionDiaryRequest>(i => i.TenancyAgreementRef.Equals(string.Empty))))
                .ReturnsAsync(new ArrearsActionResponse
                {
                    Success = false,
                    ArrearsAction = new ArrearsActionLogDto
                    {
                        TenancyAgreementRef = string.Empty
                    }

                });
            var request = new ActionDiaryRequest
            {
                    TenancyAgreementRef = string.Empty
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request);
            //assert
            Assert.False(response.Success);
        }

        [Fact]
        public void GivenNull_GatewayResponseWith_Failure()
        {
            //arrange
            _fakeGateway.Setup(s => s.CreateActionDiaryEntryAsync(It.Is<ActionDiaryRequest>(i => i == null)))
                .Throws<AggregateException>();
            //act
            //assert
            Assert.Throws<AggregateException>(()=> _classUnderTest.ExecuteAsync(null).Result);
        }
    }
}
