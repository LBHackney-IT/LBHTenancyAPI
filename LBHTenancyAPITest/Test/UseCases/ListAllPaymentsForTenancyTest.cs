using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helper;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListAllPaymentsForTenancyTest
    {
        [Fact]
        public void WhenGivenATenancyRefThatDoesntExist_ShouldReturnAnEmptyPaymentResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllPayments = new ListAllPayments(gateway);
            var response = listAllPayments.Execute("");

            Assert.IsType(typeof(ListAllPayments.PaymentTransactionResponse), response);
            Assert.Empty(response.PaymentTransactions);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnAPaymentResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllPayments = new ListAllPayments(gateway);

            var payment = Fake.GeneratePaymentTransactionDetails();
            gateway.SetPaymentTransactionDetails(payment.TenancyRef, payment);

            var response = listAllPayments.Execute(payment.TenancyRef);

            Assert.IsType(typeof(ListAllPayments.PaymentTransactionResponse), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludePaymentsForThatTenancy()
        {
            var gateway = new StubTenanciesGateway();
            var payment = Fake.GeneratePaymentTransactionDetails();

            gateway.SetPaymentTransactionDetails(payment.TenancyRef, payment);

            var listAllPayments = new ListAllPayments(gateway);
            var response = listAllPayments.Execute(payment.TenancyRef);

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
                        PropertyRef = payment.PropertyRef
                    }
                }
            };

            Assert.Equal(expectedResponse.PaymentTransactions, response.PaymentTransactions);
        }
    }
}
