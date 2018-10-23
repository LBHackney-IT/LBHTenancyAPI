using System;
using System.Threading.Tasks;
using FluentAssertions;
using AgreementService;
using LBHTenancyAPI.Gateways.V1.Arrears;
using LBHTenancyAPI.Gateways.V1.Arrears.Impl;
using LBHTenancyAPITest.Helpers;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.V1.ArrearsActions
{
    public class UHArrearsDiaryCreationGatewayTest
    {
        [Fact]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldNotBeNull()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(new ArrearsActionResponse());


            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object);

            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    ActionBalance = 10,
                    ActionCategory = "8",
                    ActionCode = "GEN",
                    Comment = "Testing",
                    TenancyAgreementRef = "000017/01"
                },
                DirectUser = new UserCredential
                {
                    UserName = "TestUserName",
                    UserPassword = "TestUserPassword"
                },
                SourceSystem = "TestSystem"
            };

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);

            //assert
            Assert.NotNull(response);
        }

        [Theory]
        [InlineData("000017/01", 10, "8", "GEN", "Webservice action")]
        [InlineData("000017/02", 17, "9", "Test", "Testing")]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldReturnAValidObject(
            string tenancyRef, decimal actionBalance, string actionCategory, string actionCode, string comment )
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object);

            var request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = tenancyRef,
                    ActionBalance = actionBalance,
                    ActionCategory = actionCategory,
                    ActionCode = actionCode,
                    Comment = comment
                    
                },
                DirectUser = new UserCredential
                {
                    UserName = "TestUserName",
                    UserPassword = "TestUserPassword"
                },
                SourceSystem = "TestSystem"
            };

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(Fake.CreateArrearsActionAsync(request));

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);
            //assert
            response.ArrearsAction.TenancyAgreementRef.Should().Be(tenancyRef);
            response.ArrearsAction.ActionBalance.Should().Be(actionBalance);
            response.ArrearsAction.ActionCategory.Should().Be(actionCategory);
            response.ArrearsAction.ActionCode.Should().Be(actionCode);
        }

        [Fact]
        public void GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithNull_ShouldThrowAnException()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object);

            //act
            //assert
            Assert.Throws<AggregateException>(()=>classUnderTest.CreateActionDiaryEntryAsync(null).Result);
        }
    }
}
