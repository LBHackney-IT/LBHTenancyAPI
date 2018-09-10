using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListAllPaymentsForTenancyTest
    {
        [Fact]
        public async Task WhenGivenATenancyRefThatDoesntExist_ShouldReturnAnEmptyPaymentResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllPayments = new ListAllPayments(gateway);
            var response = await listAllPayments.ExecuteAsync("");

            Assert.IsType(typeof(ListAllPayments.PaymentTransactionResponse), response);
            Assert.Empty(response.PaymentTransactions);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_ShouldReturnAPaymentResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllPayments = new ListAllPayments(gateway);

            var payment = Fake.GeneratePaymentTransactionDetails();
            gateway.SetPaymentTransactionDetails(payment.TenancyRef, payment);

            var response = await listAllPayments.ExecuteAsync(payment.TenancyRef);

            Assert.IsType(typeof(ListAllPayments.PaymentTransactionResponse), response);
        }

        [Fact]
        public async Task WhenATenancyRefIsGiven_ResponseShouldIncludePaymentsForThatTenancy()
        {
            var gateway = new StubTenanciesGateway();
            var payment = Fake.GeneratePaymentTransactionDetails();

            gateway.SetPaymentTransactionDetails(payment.TenancyRef, payment);

            var listAllPayments = new ListAllPayments(gateway);
            var response = await listAllPayments.ExecuteAsync(payment.TenancyRef);

            var expectedResponse = new ListAllPayments.PaymentTransactionResponse
            {
                PaymentTransactions = new List<ListAllPayments.PaymentTransaction>
                {
                    new ListAllPayments.PaymentTransaction
                    {
                        Ref= payment.TransactionRef,
                        Amount= payment.Amount.ToString("C"),
                        Date = string.Format("{0:u}", payment.Date),
                        Type = payment.Type,
                        PropertyRef = payment.PropertyRef,
                        Description = payment.Description
                    }
                }
            };

            expectedResponse.PaymentTransactions[0].Should().BeEquivalentTo(response.PaymentTransactions[0]);
        }
    }
}
