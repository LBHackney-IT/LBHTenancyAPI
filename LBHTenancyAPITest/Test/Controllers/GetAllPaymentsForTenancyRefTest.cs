using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using LBHTenancyAPI.UseCases;
using Xunit;
using LBHTenancyAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class GetAllPaymentsForTenancyRefTest
    {
        [Fact]
        public async Task WhenGivenTenancyRefThatDoesntExist_Payments_ShouldRespondWithNoResults()
        {
            var allPayments = new AllPaymentsStub();
            var response = await GetPaymentTransactionDetails(allPayments, "NotHere");
            Assert.NotNull(response);

            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject
            (
                new Dictionary<string, object> {{"payment_transactions", new List<AllPaymentsForTenancy.PaymentTransaction>()}}
            );

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_Payments_ShouldCallGetPaymentTransactionDetails()
        {
            var allPaymentsSpy = new AllPaymentsSpy();
            await GetPaymentTransactionDetails(allPaymentsSpy, "EXAMPLE/123");

            allPaymentsSpy.AssertCalledOnce();
            allPaymentsSpy.AssertCalledWith("EXAMPLE/123");
        }

        [Fact]
        public async Task WhenGivenATenancyRef_Payments_ShouldRespondWithFormattedJson()
        {
            var allPayments = new AllPaymentsStub();


            allPayments.AddPaymentTransaction("0test/01", new List<AllPaymentsForTenancy.PaymentTransaction>
            {
                new AllPaymentsForTenancy.PaymentTransaction
                {
                    PropertyRef = "000002/01/11",
                    Amount = "23.01",
                    Date = "2018-01-01 00:00:00Z",
                    Type = "Direct Debit",
                    Ref = "12345678"
                },
                new AllPaymentsForTenancy.PaymentTransaction
                {
                    PropertyRef = "000002/02/12",
                    Amount = "24.01",
                    Date = "2018-01-03 00:00:00Z",
                    Type = "Direct Debit",
                    Ref = "123456789"
                }
            });

            var response = await GetPaymentTransactionDetails(allPayments, "0test/01");

            var first = new Dictionary<string, object>
            {
                {"ref", "12345678"},
                {"amount", "23.01"},
                {"date", "2018-01-01 00:00:00Z"},
                {"type", "Direct Debit"},
                {"property_ref", "000002/01/11"}
            };

            var second = new Dictionary<string, object>
            {
                {"ref", "123456789"},
                {"amount", "24.01"},
                {"date", "2018-01-03 00:00:00Z"},
                {"type", "Direct Debit"},
                {"property_ref", "000002/02/12"}
            };

            var output = new Dictionary<string, object>
            {
                {"payment_transactions",
                    new List<Dictionary<string, object>>
                    {
                        first,
                        second
                    }

                }
            };

            var actualResponse = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject(output);

            Assert.Equal(expectedJson, actualResponse);
        }

        private static async Task<ObjectResult> GetPaymentTransactionDetails(IListAllPayments listPaymentsUseCase, string tenancyRef)
        {
            var controller = new TenanciesController(null, null, listPaymentsUseCase);
            var result = await controller.PaymentTransactionDetails(tenancyRef);
            return result as OkObjectResult;
        }

        private static string ResponseJson(ObjectResult response)
        {
            return JsonConvert.SerializeObject(response.Value);
        }

        private class AllPaymentsSpy : IListAllPayments
        {
            private readonly List<object> calledWith;

            public AllPaymentsSpy()
            {
                calledWith = new List<object>();
            }

            public AllPaymentsForTenancy.PaymentTransactionResponse Execute(string tenancyRef)
            {
                calledWith.Add(tenancyRef);
                return new AllPaymentsForTenancy.PaymentTransactionResponse {PaymentTransactions = new List<AllPaymentsForTenancy.PaymentTransaction>()};
            }

            public void AssertCalledOnce()
            {
                Assert.Single(calledWith);
            }

            public void AssertCalledWith(object expectedArgument)
            {
                Assert.Equal(expectedArgument, calledWith[0]);
            }
        }

        private class AllPaymentsStub : IListAllPayments
        {
            private readonly Dictionary<string, List<AllPaymentsForTenancy.PaymentTransaction>> stubPaymentsTransactionsDetails;

            public AllPaymentsStub()
            {
                stubPaymentsTransactionsDetails = new Dictionary<string, List<AllPaymentsForTenancy.PaymentTransaction>>();
            }

            public void AddPaymentTransaction(string tenancyRef, List<AllPaymentsForTenancy.PaymentTransaction> paymentTransactions)
            {
                stubPaymentsTransactionsDetails[tenancyRef] = paymentTransactions;
            }

            public AllPaymentsForTenancy.PaymentTransactionResponse Execute(string tenancyRef)
            {
                var savedPayments = new List<AllPaymentsForTenancy.PaymentTransaction>();

                if (stubPaymentsTransactionsDetails.ContainsKey(tenancyRef))
                {
                    savedPayments = stubPaymentsTransactionsDetails[tenancyRef];
                }


                return new AllPaymentsForTenancy.PaymentTransactionResponse
                {
                    PaymentTransactions = savedPayments
                };
            }
        }


    }
}
