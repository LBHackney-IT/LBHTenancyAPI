using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using FluentAssertions;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases.ArrearsActions
{
    public class CreateArrearsAgreementUseCaseTests
    {
        private ICreateArrearsAgreementUseCase _classUnderTest;
        private Mock<IArrearsAgreementGateway> _fakeGateway;

        public CreateArrearsAgreementUseCaseTests()
        {
            _fakeGateway = new Mock<IArrearsAgreementGateway>();
            _classUnderTest = new CreateArrearsAgreementUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidedInput_GatewayReceivesCorrectInput()
        {
            //arrange
            _fakeGateway.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>(),CancellationToken.None))
                .ReturnsAsync(new ExecuteWrapper<ArrearsAgreementResponse>(new ArrearsAgreementResponse
                {
                    Success = true
                }) );

            var request = new CreateArrearsAgreementRequest
            {
                AgreementInfo = new ArrearsAgreementInfo
                {
                    Reference = "ref"
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>()
            };
            //act
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.CreateArrearsAgreementAsync(It.Is<ArrearsAgreementRequest>(i => i.Agreement.Reference.Equals("ref")), CancellationToken.None));
        }

        [Fact]
        public async Task GivenValidedInput_GatewayResponseWith_Success()
        {
            //arrange
            _fakeGateway.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>(), CancellationToken.None))
                .ReturnsAsync(new ExecuteWrapper<ArrearsAgreementResponse>(new ArrearsAgreementResponse
                {
                    Success = true
                }));

            var request = new CreateArrearsAgreementRequest
            {
                AgreementInfo = new ArrearsAgreementInfo
                {
                    Reference = "ref"
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>()
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response?.Result?.Agreement.Reference.Should().Be("ref");
        }

        [Fact]
        public async Task GivenInvalidInput_GatewayResponseWith_Failure()
        {
            //arrange
            _fakeGateway.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>(), CancellationToken.None))
                .ReturnsAsync(new ExecuteWrapper<ArrearsAgreementResponse>(new ArrearsAgreementResponse
                {
                    Success = false,
                    
                }));

            var request = new CreateArrearsAgreementRequest
            {
                
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.IsSuccess.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Error.Should().NotBeNull();
            response.Error.IsValid.Should().BeFalse();
            response.Error.ValidationErrors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GivenNull_ShouldReturnValidationErrors()
        {
            //arrange            
            //act
            var response = await _classUnderTest.ExecuteAsync(null, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.IsSuccess.Should().BeFalse();
            response.Error.Should().NotBeNull();
            response.Error.IsValid.Should().BeFalse();
            response.Error.ValidationErrors.Should().NotBeNull();
            response.Error.ValidationErrors.Should().NotContainNulls();
        }

        [Fact]
        public async Task GivenValidInput_ThenRequestBuilder_AddsCredentials_ToRequest()
        {
            //arrange
            var reference = "ref";
            _fakeGateway.Setup(s => s.CreateArrearsAgreementAsync(It.Is<ArrearsAgreementRequest>(i => i.Agreement.Reference.Equals("ref")), CancellationToken.None))
                .ReturnsAsync(new ExecuteWrapper<ArrearsAgreementResponse>(new ArrearsAgreementResponse
                {
                    Success = true,
                    Agreement = new ArrearsAgreementDto
                    {
                        Reference = reference
                    }

                }));
            var request = new CreateArrearsAgreementRequest
            {
                AgreementInfo = new ArrearsAgreementInfo
                {
                    Reference = "ref"
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>()
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.CreateArrearsAgreementAsync(It.Is<ArrearsAgreementRequest>(i => i.DirectUser != null && !string.IsNullOrEmpty(i.SourceSystem)), CancellationToken.None));
        }
    }
}
