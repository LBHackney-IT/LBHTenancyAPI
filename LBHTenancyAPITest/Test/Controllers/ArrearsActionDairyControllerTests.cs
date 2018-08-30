using System;
using System.Threading.Tasks;
using Xunit;
using LBHTenancyAPI.Controllers;
using AgreementService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LBHTenancyAPI.UseCases.ArrearsActions;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class ArrearsActionDairyControllerTests
    {

        [Fact]
        public async Task WhenProvidedWithCorrectParamaters_ApiResponseWith200()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);
            fakeUseCase.Setup(a => a.CreateActionDiaryRecordsAsync(It.IsAny<ArrearsActionCreateRequest>())).ReturnsAsync(new ArrearsActionResponse
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
        public async Task WhenProvidedWithCorrectParamaters_AndThereIsErrorFromWebService_ApiResponseWith500()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);
            fakeUseCase.Setup(a => a.CreateActionDiaryRecordsAsync(It.IsAny<ArrearsActionCreateRequest>())).ReturnsAsync(new ArrearsActionResponse
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
        public async Task WhenProvidedWithIncorrectParameters_AndThereIsErrorFromWebService_ApiResponseWith400()
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
