using System;
using Xunit;
using LBHTenancyAPI.Gateways;
using AgreementService;
using Moq;
using System.Threading.Tasks;
using LBHTenancyAPI.Interfaces;
using LBHTenancyAPITest.Helpers;

namespace LBHTenancyAPITest.Test.Gateways
{
    public class UHArrearsDiaryCreationGatewayTest
    {
        [Fact]
        public async Task Given_TenancyAgreementRef_When_CreateActionDiaryEntry_WithCorrectParameters_ShouldNotBeNull()
        {
            //Arrange
            Mock<IArrearsAgreementService> fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

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
                    UserName = "HackneyAPI",
                    UserPassword = "Hackney1"
                },
                SourceSystem = "HackneyAPI"
            };

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);

            //assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Given_TenancyAgreementRef_When_CreateActionDiaryEntry_WithCorrectParameters_ShouldReturnAValidObject()
        {
            //Arrange
            Mock<IArrearsAgreementService> fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

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
                    UserName = "HackneyAPI",
                    UserPassword = "Hackney1"
                },
                SourceSystem = "HackneyAPI"
            };

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsActionAsync(It.IsAny<ArrearsActionCreateRequest>()))
                .ReturnsAsync(Fake.CreateArrearsActionAsync(request));

            //act
            var response = await classUnderTest.CreateActionDiaryEntryAsync(request);
            //assert
            Assert.Equal(response.ArrearsAction.TenancyAgreementRef, request.ArrearsAction.TenancyAgreementRef);
        }

        [Fact]
        public void Given_TenancyAgreementRef_When_CreateActionDiaryEntry_WithNull_ShouldThrowAnException()
        {
            //Arrange
            Mock<IArrearsAgreementService> fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            IArrearsActionDiaryGateway classUnderTest = new ArrearsActionDiaryGateway(fakeArrearsAgreementService.Object);

            //act
            //assert
            Assert.Throws<AggregateException>(()=>classUnderTest.CreateActionDiaryEntryAsync(null).Result);
        }
    }
}
