using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPITest.Helpers;
using Xunit;

namespace LBHTenancyAPITest.Test.UseCases
{
    public class ListAllPaymentsForTenancyTest
    {
        [Fact]
        public void WhenGivenATenancyRefThatDoesntExist_ShouldReturnAnEmptyPaymentResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllPayments = new AllPaymentsForTenancy(gateway);
            var response = listAllPayments.Execute("");

            Assert.IsType(typeof(AllPaymentsForTenancy.PaymentTransactionResponse), response);
            Assert.Empty(response.PaymentTransactions);
        }

        [Fact]
        public void WhenGivenATenancyRef_ShouldReturnAPaymentResponse()
        {
            var gateway = new StubTenanciesGateway();
            var listAllPayments = new AllPaymentsForTenancy(gateway);

            var payment = Fake.GeneratePaymentTransactionDetails();
            gateway.SetPaymentTransactionDetails(payment.TenancyRef, payment);

            var response = listAllPayments.Execute(payment.TenancyRef);

            Assert.IsType(typeof(AllPaymentsForTenancy.PaymentTransactionResponse), response);
        }

        [Fact]
        public void WhenATenancyRefIsGiven_ResponseShouldIncludePaymentsForThatTenancy_Example1()
        {
            var gateway = new StubTenanciesGateway();
            var payment = Fake.GeneratePaymentTransactionDetails();

            gateway.SetPaymentTransactionDetails(payment.TenancyRef, payment);

            var listAllPayments = new AllPaymentsForTenancy(gateway);
            var response = listAllPayments.Execute(payment.TenancyRef);

            var expectedResponse = new AllPaymentsForTenancy.PaymentTransactionResponse
            {
                PaymentTransactions = new List<AllPaymentsForTenancy.PaymentTransaction>
                {
                    new AllPaymentsForTenancy.PaymentTransaction
                    {
                        Ref= payment.TransactionRef,
                        Amount= payment.TransactionAmount.ToString("C"),
                        Date = string.Format("{0:u}", payment.TransactionDate),
                        Type = payment.TransactionType,
                        PropertyRef = payment.PropertyRef
                    }
                }
            };

            Assert.Equal(expectedResponse.PaymentTransactions, response.PaymentTransactions);
        }
    }
}
