using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using AgreementService;
using LBHTenancyAPI.Gateways.V1.Arrears;
using LBHTenancyAPI.Gateways.V1.Arrears.Impl;
using LBHTenancyAPI.Gateways.V1.Arrears.UniversalHousing.Impl;
using LBHTenancyAPI.Services;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.V1.ArrearsActions
{
    public class UHArrearsAgreementGatewayTest
    {
        [Fact]
        public async Task GivenTenancyAgreementRef_WhenCreateAgreementsWithCorrectParameters_ShouldNotBeNull()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse());

            var fakeCredentialsService = new Mock<ICredentialsService>();
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway classUnderTest = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object,serviceRequestBuilder );

            var request = new ArrearsAgreementRequest
            {
                Agreement = new ArrearsAgreementInfo
                {
                    TenancyAgreementRef = "000017/01",
                    Comment = "Testing",
                },
            };

            //act
            var response = await classUnderTest.CreateArrearsAgreementAsync(request,  CancellationToken.None).ConfigureAwait(false);

            //assert
            response.Should().NotBeNull();
        }

        [Theory]
        [InlineData("000017/01", "New Agreement", 400.00, "2018-08-18 14:59:00Z", "200", false, 10, "8", 1, "1", "2018-11-08 14:59:00", "TOT",
            100.00, "1", "2018-09-01 14:59:00", "Test124")]
        [InlineData("000017/02", "New Agreemenw", 500.00, "2018-09-18 14:59:00Z", "200", false, 10, "8", 1, "1", "2018-12-08 14:59:00", "TOT",
            100.00, "1", "2018-09-01 14:59:00", "Test123")]
        public async Task GivenTenancyAgreementRef_WhenCreateAgreementsWithCorrectParameters_ShouldReturnAValidObject(
            string tenancyRef,  string comment, decimal startBalance, string startDate, string agreementStatusCode,
            bool isBreached,int firstCheck,string firstCheckFrequencyTypeCode,int nextCheck,string nextCheckFrequencyTypeCode,
            string fcaDate,string monitorBalanceCode, decimal amount,string arrearsFrequencyCode,
            string payementInfoStartDate,string payemntInfoComments)
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            fakeArrearsAgreementService.Setup(s => s.CreateArrearsAgreementAsync(It.IsAny<ArrearsAgreementRequest>()))
                .ReturnsAsync(new ArrearsAgreementResponse());

            var fakeCredentialsService = new Mock<ICredentialsService>();
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway classUnderTest = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object, serviceRequestBuilder);

            var request = new ArrearsAgreementRequest
            {
                Agreement = new ArrearsAgreementInfo
                {

                    TenancyAgreementRef = tenancyRef,
                    Comment = comment,
                    ArrearsAgreementStatusCode= agreementStatusCode,
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
            var response = await classUnderTest.CreateArrearsAgreementAsync(request, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Result.Agreement.TenancyAgreementRef.Should().Be(tenancyRef);
            response.Result.Agreement.Comment.Should().Be(comment);
            response.Result.Agreement.ArrearsAgreementStatusCode.Should().Be(agreementStatusCode);
            response.Result.Agreement.FcaDate.Should().Be(DateTime.Parse(fcaDate));
            response.Result.Agreement.FirstCheck.Should().Be(firstCheck);
            response.Result.Agreement.FirstCheckFrequencyTypeCode.Should().Be(firstCheckFrequencyTypeCode);
            response.Result.Agreement.IsBreached.Should().Be(isBreached);
            response.Result.Agreement.MonitorBalanceCode.Should().Be(monitorBalanceCode);
            response.Result.Agreement.NextCheck.Should().Be(nextCheck);
            response.Result.Agreement.NextCheckFrequencyTypeCode.Should().Be(nextCheckFrequencyTypeCode);
            response.Result.Agreement.StartBalance.Should().Be(startBalance);
            response.Result.Agreement.StartDate.Should().Be(DateTime.Parse(startDate));

            response.Result.Agreement.PaymentSchedule.Should().NotBeNull();
            var paymentSchedule = response.Result.Agreement.PaymentSchedule[0];
            paymentSchedule.Amount.Should().Be(amount);
            paymentSchedule.ArrearsFrequencyCode.Should().Be(arrearsFrequencyCode);
            paymentSchedule.Comments.Should().Be(payemntInfoComments);
            paymentSchedule.StartDate.Should().Be(DateTime.Parse(payementInfoStartDate));
        }

        [Fact]
        public void GivenTenancyAgreementRef_WhenCreateArrearsAgreementWithNull_ShouldThrowAnException()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementServiceChannel>();

            var fakeCredentialsService = new Mock<ICredentialsService>();
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway classUnderTest = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object, serviceRequestBuilder);

            //act
            //assert
            Assert.Throws<AggregateException>(() => classUnderTest.CreateArrearsAgreementAsync(null, CancellationToken.None).Result);
        }
    }
}
