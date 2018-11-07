using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Controllers.V2;
using LBHTenancyAPI.UseCases.V2.ArrearsActions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers.V2
{
    public class ArrearsActionDairyControllerTests
    {

        [Fact]
        public async Task WhenGivenCorrectParamaters_ApiShouldRespondWith200()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);
            fakeUseCase.Setup(a => a.ExecuteAsync(It.IsAny<ArrearsActionCreateRequest>())).ReturnsAsync(new ArrearsActionResponse
            {
                Success = true
            });

            //Act
            ArrearsActionCreateRequest request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = "s",
                    ActionBalance = 1,
                    Comment = "test",
                    ActionCode = "t1"
                }
            };

            var response = await classUnderTest.Post(request);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task WhenGivenCorrectParamaters_AndThereIsErrorFromWebService_ApiShouldRespondWith500()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);
            fakeUseCase.Setup(a => a.ExecuteAsync(It.IsAny<ArrearsActionCreateRequest>())).ReturnsAsync(new ArrearsActionResponse
            {
                Success = false
            });

            //Act
            ArrearsActionCreateRequest request = new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = "s",
                    ActionBalance = 1,
                    Comment = "test",
                    ActionCode = "t1"
                }
            };

            var response = await classUnderTest.Post(request);

            //Assert
            Assert.IsType<ObjectResult>(response);
        }

        [Fact]
        public async Task WhenGivenIncorrectParameters_AndThereIsErrorFromWebService_ApiShouldRespondWith400()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);

            //Act
            var response = await classUnderTest.Post(null);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }
    }
}
