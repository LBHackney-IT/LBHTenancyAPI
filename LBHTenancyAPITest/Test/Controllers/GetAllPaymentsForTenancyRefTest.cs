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
                new Dictionary<string, object> {{"payment_transactions", new List<ListAllPayments.PaymentTransaction>()}}
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
        public async Task WhenGivenATenancyRef_Payments_ShouldRespondWithFormattedJson_Example1()
        {
            var allPayments = new AllPaymentsStub();
            allPayments.AddPaymentTransaction("0test/01", new List<ListAllPayments.PaymentTransaction>
            {
                new ListAllPayments.PaymentTransaction
                {
                    PropertyRef = "000002/01/11",
                    Amount = "23.01",
                    Date = "2018-01-01 00:00:00Z",
                    Type = "Direct Debit",
                    Ref = "12345678"
                },
                new ListAllPayments.PaymentTransaction
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

        [Fact]
        public async Task WhenGivenATenancyRef_Payments_ShouldRespondWithFormattedJson_Example2()
        {
            var allPayments = new AllPaymentsStub();


            allPayments.AddPaymentTransaction("3test/32", new List<ListAllPayments.PaymentTransaction>
            {
                new ListAllPayments.PaymentTransaction
                {
                    PropertyRef = "010101/02/99",
                    Amount = "459.99",
                    Date = "2017-11-30 00:00:00Z",
                    Type = "PayPoint",
                    Ref = "6645352"
                },
                new ListAllPayments.PaymentTransaction
                {
                    PropertyRef = "33333/55/77",
                    Amount = "32.22",
                    Date = "2018-02-23 00:00:00Z",
                    Type = "type!",
                    Ref = "098765"
                }
            });

            var response = await GetPaymentTransactionDetails(allPayments, "3test/32");

            var first = new Dictionary<string, object>
            {
                {"ref", "6645352"},
                {"amount", "459.99"},
                {"date", "2017-11-30 00:00:00Z"},
                {"type", "PayPoint"},
                {"property_ref", "010101/02/99"}
            };

            var second = new Dictionary<string, object>
            {
                {"ref", "098765"},
                {"amount", "32.22"},
                {"date", "2018-02-23 00:00:00Z"},
                {"type", "type!"},
                {"property_ref", "33333/55/77"}
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
            var controller = new TenanciesController(null, null, listPaymentsUseCase, null);
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

            public ListAllPayments.PaymentTransactionResponse Execute(string tenancyRef)
            {
                calledWith.Add(tenancyRef);
                return new ListAllPayments.PaymentTransactionResponse {PaymentTransactions = new List<ListAllPayments.PaymentTransaction>()};
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
            private readonly Dictionary<string, List<ListAllPayments.PaymentTransaction>> stubPaymentsTransactionsDetails;

            public AllPaymentsStub()
            {
                stubPaymentsTransactionsDetails = new Dictionary<string, List<ListAllPayments.PaymentTransaction>>();
            }

            public void AddPaymentTransaction(string tenancyRef, List<ListAllPayments.PaymentTransaction> paymentTransactions)
            {
                stubPaymentsTransactionsDetails[tenancyRef] = paymentTransactions;
            }

            public ListAllPayments.PaymentTransactionResponse Execute(string tenancyRef)
            {
                var savedPayments = new List<ListAllPayments.PaymentTransaction>();

                if (stubPaymentsTransactionsDetails.ContainsKey(tenancyRef))
                {
                    savedPayments = stubPaymentsTransactionsDetails[tenancyRef];
                }


                return new ListAllPayments.PaymentTransactionResponse
                {
                    PaymentTransactions = savedPayments
                };
            }
        }


    }
}
