using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using FluentAssertions;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;
using LBHTenancyAPI.Infrastructure.Validation;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using LBHTenancyAPI.UseCases.ArrearsAgreements.Models;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class ContactsControllerTests
    {
        private ContactController _classUnderTest;
        private Mock<IGetContactsForTenancyUseCase> _mock;

        public ContactsControllerTests()
        {
            _mock = new Mock<IGetContactsForTenancyUseCase>();
            _classUnderTest = new ContactController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidCreateArrearsAgreementRequest_ShouldReturn200()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<GetContactsForTenancyRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetContactsForTenancyResponse
                {
                    Contacts = new List<ContactsForTenancy>
                    {

                    }
                });

            var request = new GetContactsForTenancyRequest
            {
                TenancyAgreementReference = "test"
            };
            //act
            var response = await _classUnderTest.Get(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
        }

        //[Fact]
        //public async Task WhenGivenIncorrectParameters_AndThereIsErrorFromWebService_ApiShouldRespondWith400()
        //{
        //    //Arrange
        //    _mock.Setup(s => s.ExecuteAsync(It.IsAny<CreateArrearsAgreementRequest>(), CancellationToken.None))
        //        .ReturnsAsync(new ExecuteWrapper<CreateArrearsAgreementResponse>(new RequestValidationResponse(false)));
        //    //Act
        //    var response = await _classUnderTest.Post(null);

        //    //Assert
        //    Assert.IsType<ObjectResult>(response);
        //    var responseResult = response as ObjectResult;
        //    responseResult.StatusCode.Should().Be(400);
        //    responseResult.Value.Should().NotBeNull();
        //    var apiResponse = responseResult.Value as APIResponse<CreateArrearsAgreementResponse>;
        //    apiResponse.Data.Should().BeNull();
        //    apiResponse.Error.IsValid.Should().BeFalse();
        //    apiResponse.Error.ValidationErrors.Should().NotBeNull();
        //}


        //[Fact]
        //public async Task WhenGivenCorrectParamaters_AndThereIsExceptionFromWebService_ApiShouldRespondWith500()
        //{
        //    //Arrange

        //    _mock.Setup(s => s.ExecuteAsync(It.IsAny<CreateArrearsAgreementRequest>(), CancellationToken.None))
        //        .ReturnsAsync(new ExecuteWrapper<CreateArrearsAgreementResponse>(new ArrearsAgreementResponse
        //        {
        //            Success = false,
        //            ErrorMessage = "there is no field",
        //            ErrorCode = 1
        //        }));

        //    //Act
        //    var request = new CreateArrearsAgreementRequest
        //    {
        //        AgreementInfo = new ArrearsAgreementInfo
        //        {

        //            TenancyAgreementRef = "s",
        //            Comment = "testing",
        //            ArrearsAgreementStatusCode = "1",

        //        },
        //        PaymentSchedule = new List<ArrearsScheduledPaymentInfo>
        //        {
        //            new ArrearsScheduledPaymentInfo
        //            {
        //                Amount = 10,
        //                ArrearsFrequencyCode = "200",
        //                Comments = "testing",

        //            }
        //        }.ToArray()
        //    };

        //    var response = await _classUnderTest.Post(request);

        //    //Assert
        //    Assert.IsType<ObjectResult>(response);
        //    var responseResult = response as ObjectResult;
        //    responseResult.StatusCode.Should().Be(500);
        //    responseResult.Value.Should().NotBeNull();
        //    var apiResponse = responseResult.Value as APIResponse<CreateArrearsAgreementResponse>;
        //    apiResponse.Data.Should().BeNull();
        //    apiResponse.Error.Errors.Should().NotBeNull();
        //    apiResponse.Error.Errors[0].Code = "UH_1";
        //    apiResponse.Error.Errors[0].Message = "there is no field";
        //}
    }
}
