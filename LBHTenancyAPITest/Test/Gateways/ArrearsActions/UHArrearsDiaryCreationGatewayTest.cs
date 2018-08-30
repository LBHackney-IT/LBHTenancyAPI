using System;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPITest.Helpers;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.ArrearsActions
{
    public class UHArrearsDiaryCreationGatewayTest
    {
        [Fact]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldNotBeNull()
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
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldReturnAValidObject()
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
        public void GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithNull_ShouldThrowAnException()
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
