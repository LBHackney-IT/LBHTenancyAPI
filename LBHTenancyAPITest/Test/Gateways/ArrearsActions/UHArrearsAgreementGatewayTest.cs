using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using AgreementService;
using LBHTenancyAPI.Gateways.Arrears;
using LBHTenancyAPI.Gateways.Arrears.Impl;
using LBHTenancyAPI.Services;
using Moq;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.ArrearsActions
{
    public class UHArrearsAgreementGatewayTest
    {
        [Fact]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldNotBeNull()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

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
        [InlineData("000017/01", "New Agreement", 400.00 )]
        [InlineData("000017/02", "Testing", 500.00)]
        public async Task GivenTenancyAgreementRef_WhenCreateActionDiaryEntryWithCorrectParameters_ShouldReturnAValidObject(
            string tenancyRef,  string comment, decimal amount)
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

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
                    
                },
                PaymentSchedule = new List<ArrearsScheduledPaymentInfo>
                {
                    new ArrearsScheduledPaymentInfo
                    {
                        Amount = amount
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
                        PaymentSchedule = new List<ArrearsScheduledPaymentDto>
                        {
                            new ArrearsScheduledPaymentDto
                            {
                                Amount = amount,
                                //ArrearsFrequencyCode = 
                            }
                        }.ToArray()
                    },
                });

            //act
            var response = await classUnderTest.CreateArrearsAgreementAsync(request, CancellationToken.None).ConfigureAwait(false);
            //assert
            response.Result.Agreement.TenancyAgreementRef.Should().Be(tenancyRef);
            response.Result.Agreement.Comment.Should().Be(comment);
            response.Result.Agreement.PaymentSchedule.Should().NotBeNull();
            response.Result.Agreement.PaymentSchedule[0].Amount.Should().Be(amount);
        }

        [Fact]
        public void GivenTenancyAgreementRef_WhenCreateArrearsAgreementWithNull_ShouldThrowAnException()
        {
            //Arrange
            var fakeArrearsAgreementService = new Mock<IArrearsAgreementService>();

            var fakeCredentialsService = new Mock<ICredentialsService>();
            var serviceRequestBuilder = new ArrearsServiceRequestBuilder(fakeCredentialsService.Object);

            IArrearsAgreementGateway classUnderTest = new ArrearsAgreementGateway(fakeArrearsAgreementService.Object, serviceRequestBuilder);

            //act
            //assert
            Assert.Throws<AggregateException>(() => classUnderTest.CreateArrearsAgreementAsync(null, CancellationToken.None).Result);
        }
    }
}
