using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using FluentAssertions;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Gateways.Arrears;
using LBHTenancyAPI.Gateways.Arrears.Impl;
using LBHTenancyAPI.Services;
using LBHTenancyAPI.Services.Impl;
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
            response.Error.ValidationErrors.Should().NotBeNullOrEmpty();
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
            response.Error.ValidationErrors.Should().NotBeNullOrEmpty();
        }


        [Theory]
        [InlineData("000017/01", "New Agreement", 400.00, "2018-08-18 14:59:00Z", "200", false, 10, "8", 1, "1", "2018-11-08 14:59:00", "TOT",
            100.00, "1", "2018-09-01 14:59:00", "Test124")]
        public async Task GivenValidInput_ThenRequestBuilder_AddsCredentials_ToRequest(
            string tenancyRef, string comment, decimal startBalance, string startDate, string agreementStatusCode,
            bool isBreached, int firstCheck, string firstCheckFrequencyTypeCode, int nextCheck, string nextCheckFrequencyTypeCode,
            string fcaDate, string monitorBalanceCode, decimal amount, string arrearsFrequencyCode,
            string payementInfoStartDate, string payemntInfoComments)
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse());

            var fakeCredentialsService = new Mock<ICredentialsService>();
            fakeCredentialsService.Setup(s => s.GetUhSourceSystem()).Returns("testSourceSystem");
            fakeCredentialsService.Setup(s => s.GetUhUserCredentials()).Returns(new UserCredential
            {
                UserName = "TestUserName",
                UserPassword = "TestUserPassword",
            });
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway gateway = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object, serviceRequestBuilder);
            var classUnderTest = new CreateArrearsAgreementUseCase(gateway);

            var request = new CreateArrearsAgreementRequest
            {
                
                AgreementInfo = new ArrearsAgreementInfo
                {

                    TenancyAgreementRef = tenancyRef,
                    Comment = comment,
                    ArrearsAgreementStatusCode = agreementStatusCode,
                    FcaDate = DateTime.Parse(fcaDate),
                    FirstCheck = firstCheck,
                    FirstCheckFrequencyTypeCode = firstCheckFrequencyTypeCode,
                    IsBreached = isBreached,
                    MonitorBalanceCode = monitorBalanceCode,
                    NextCheck = nextCheck,
                    NextCheckFrequencyTypeCode = nextCheckFrequencyTypeCode,
                    StartBalance = startBalance,
                    StartDate = DateTime.Parse(startDate)
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>
                {
                    new ArrearsScheduledPaymentInfo
                    {
                        Amount = amount,
                        ArrearsFrequencyCode = arrearsFrequencyCode,
                        Comments = payemntInfoComments,
                        StartDate = DateTime.Parse(payementInfoStartDate)
                    }
                }.ToArray()
            };

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse
                {
                    Success = true,
                    Agreement = new ArrearsAgreementDto
                    {
                        TenancyAgreementRef = tenancyRef,
                        Comment = comment,
                        ArrearsAgreementStatusCode = agreementStatusCode,
                        FcaDate = DateTime.Parse(fcaDate),
                        FirstCheck = firstCheck,
                        FirstCheckFrequencyTypeCode = firstCheckFrequencyTypeCode,
                        IsBreached = isBreached,
                        MonitorBalanceCode = monitorBalanceCode,
                        NextCheck = nextCheck,
                        NextCheckFrequencyTypeCode = nextCheckFrequencyTypeCode,
                        StartBalance = startBalance,
                        StartDate = DateTime.Parse(startDate),

                        PaymentSchedule = new List<ArrearsScheduledPaymentDto>
                        {
                            new ArrearsScheduledPaymentDto
                            {
                                Amount = amount,
                                ArrearsFrequencyCode = arrearsFrequencyCode,
                                Comments = payemntInfoComments,
                                StartDate = DateTime.Parse(payementInfoStartDate)
                            }
                        }.ToArray()
                    },
                });
            //act
            await classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            fakeArrearsAgreementService.Verify(v => v.CreateArrearsAgreementAsync(It.Is<ArrearsAgreementRequest>(i => i.DirectUser != null && !string.IsNullOrEmpty(i.SourceSystem))));
        }

        [Theory]
        [InlineData("000017/01", "New Agreement", 400.00, "2018-08-18 14:59:00Z", "200", false, 10, "8", 1, "1", "2018-11-08 14:59:00", "TOT",
    100.00, "1", "2018-09-01 14:59:00", "Test124")]
        public async Task GivenValidInput_ThenUseCase_ShouldReturnValidResponse(
    string tenancyRef, string comment, decimal startBalance, string startDate, string agreementStatusCode,
    bool isBreached, int firstCheck, string firstCheckFrequencyTypeCode, int nextCheck, string nextCheckFrequencyTypeCode,
    string fcaDate, string monitorBalanceCode, decimal amount, string arrearsFrequencyCode,
    string payementInfoStartDate, string payemntInfoComments)
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse
                {
                    Success = true,
                    Agreement = new ArrearsAgreementDto
                    {
                        TenancyAgreementRef = tenancyRef,
                        Comment = comment,
                        ArrearsAgreementStatusCode = agreementStatusCode,
                        FcaDate = DateTime.Parse(fcaDate),
                        FirstCheck = firstCheck,
                        FirstCheckFrequencyTypeCode = firstCheckFrequencyTypeCode,
                        IsBreached = isBreached,
                        MonitorBalanceCode = monitorBalanceCode,
                        NextCheck = nextCheck,
                        NextCheckFrequencyTypeCode = nextCheckFrequencyTypeCode,
                        StartBalance = startBalance,
                        StartDate = DateTime.Parse(startDate),

                        PaymentSchedule = new List<ArrearsScheduledPaymentDto>
                        {
                            new ArrearsScheduledPaymentDto
                            {
                                Amount = amount,
                                ArrearsFrequencyCode = arrearsFrequencyCode,
                                Comments = payemntInfoComments,
                                StartDate = DateTime.Parse(payementInfoStartDate)
                            }
                        }.ToArray()
                    },
                });

            var fakeCredentialsService = new Mock<ICredentialsService>();
            fakeCredentialsService.Setup(s => s.GetUhSourceSystem()).Returns("testSourceSystem");
            fakeCredentialsService.Setup(s => s.GetUhUserCredentials()).Returns(new UserCredential
            {
                UserName = "TestUserName",
                UserPassword = "TestUserPassword",
            });
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway gateway = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object, serviceRequestBuilder);
            var classUnderTest = new CreateArrearsAgreementUseCase(gateway);

            var request = new CreateArrearsAgreementRequest
            {

                AgreementInfo = new ArrearsAgreementInfo
                {

                    TenancyAgreementRef = tenancyRef,
                    Comment = comment,
                    ArrearsAgreementStatusCode = agreementStatusCode,
                    FcaDate = DateTime.Parse(fcaDate),
                    FirstCheck = firstCheck,
                    FirstCheckFrequencyTypeCode = firstCheckFrequencyTypeCode,
                    IsBreached = isBreached,
                    MonitorBalanceCode = monitorBalanceCode,
                    NextCheck = nextCheck,
                    NextCheckFrequencyTypeCode = nextCheckFrequencyTypeCode,
                    StartBalance = startBalance,
                    StartDate = DateTime.Parse(startDate)
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>
                {
                    new ArrearsScheduledPaymentInfo
                    {
                        Amount = amount,
                        ArrearsFrequencyCode = arrearsFrequencyCode,
                        Comments = payemntInfoComments,
                        StartDate = DateTime.Parse(payementInfoStartDate)
                    }
                }.ToArray()
            };


            //act
            var response = await classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.IsSuccess.Should().BeTrue();
            response.Result.Should().NotBeNull();
        }

        [Theory]
        [InlineData("000017/01", "New Agreement", 400.00, "2018-08-18 14:59:00Z", "200", false, 10, "8", 1, "1", "2018-11-08 14:59:00", "TOT",
            100.00, "1", "2018-09-01 14:59:00", "Test124")]
        [InlineData("000017/01", "New Agreement2", 500.00, "2018-08-18 15:00:00Z", "300", false, 10, "8", 1, "1", "2018-11-08 14:59:00", "TOT",
            300.00, "1", "2018-09-01 15:00:00", "Test125")]
        public async Task GivenInValidInput_ThenUseCase_ShouldReturnInValidResponse(
            string tenancyRef, string comment, decimal startBalance, string startDate, string agreementStatusCode,
            bool isBreached, int firstCheck, string firstCheckFrequencyTypeCode, int nextCheck, string nextCheckFrequencyTypeCode,
            string fcaDate, string monitorBalanceCode, decimal amount, string arrearsFrequencyCode,
            string payementInfoStartDate, string payemntInfoComments)
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse
                {
                    Success = false,
                    ErrorCode = 1,
                    ErrorMessage = "Not enough field",
                    Agreement = new ArrearsAgreementDto
                    {
                        TenancyAgreementRef = tenancyRef,
                        Comment = comment,
                    },
                });

            var fakeCredentialsService = new Mock<ICredentialsService>();
            fakeCredentialsService.Setup(s => s.GetUhSourceSystem()).Returns("testSourceSystem");
            fakeCredentialsService.Setup(s => s.GetUhUserCredentials()).Returns(new UserCredential
            {
                UserName = "TestUserName",
                UserPassword = "TestUserPassword",
            });
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway gateway = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object, serviceRequestBuilder);
            var classUnderTest = new CreateArrearsAgreementUseCase(gateway);

            var request = new CreateArrearsAgreementRequest
            {

                AgreementInfo = new ArrearsAgreementInfo
                {

                    TenancyAgreementRef = tenancyRef,
                    Comment = comment,
                    ArrearsAgreementStatusCode = agreementStatusCode,
                    FcaDate = DateTime.Parse(fcaDate),
                    FirstCheck = firstCheck,
                    FirstCheckFrequencyTypeCode = firstCheckFrequencyTypeCode,
                    IsBreached = isBreached,
                    MonitorBalanceCode = monitorBalanceCode,
                    NextCheck = nextCheck,
                    NextCheckFrequencyTypeCode = nextCheckFrequencyTypeCode,
                    StartBalance = startBalance,
                    StartDate = DateTime.Parse(startDate)
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>
                {
                    new ArrearsScheduledPaymentInfo
                    {
                        Amount = amount,
                        ArrearsFrequencyCode = arrearsFrequencyCode,
                        Comments = payemntInfoComments,
                        StartDate = DateTime.Parse(payementInfoStartDate)
                    }
                }.ToArray()
            };

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse
                {
                    Success = false,
                    ErrorCode = 1,
                    ErrorMessage = "Not enough field",
                });


            //act
            var response = await classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.IsSuccess.Should().BeFalse();
            response.Result.Should().BeNull();
            response.Error.Errors.Should().NotBeNullOrEmpty();
            response.Error.Errors[0].Code.Should().Be("UH_1");
            response.Error.Errors[0].Message.Should().Be("Not enough field");
        }
    }
}
