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
    public class TenancyTest
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

        [Fact]
        public async Task WhenGivenTenancyRefThatDoesntExist_ActionDiary_ShouldRespondWithNoResults()
        {
            var allActions = new AllActionsStub();
            var response = await GetArrearsActionsDetails(allActions, "NotHere");
            Assert.NotNull(response);

            var actualJson = ResponseJson(response);
            var expectedJson = JsonConvert.SerializeObject
            (
                new Dictionary<string, object> {{"arrears_action_diary_events", new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>()}}
            );

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public async Task WhenGivenATenancyRef_ActionDiary_ShouldCallGetActionDiaryDetails()
        {
            var allActionDiarySpy = new AllActionDiarySpy();
            await GetArrearsActionsDetails(allActionDiarySpy, "EXAMPLE/123");

            allActionDiarySpy.AssertCalledOnce();
            allActionDiarySpy.AssertCalledWith("EXAMPLE/123");
        }

        [Fact]
        public async Task WhenGivenATenancyRef_ActionDiary_ShouldRespondWithFormattedJson()
        {
            var allActions = new AllActionsStub();

            allActions.AddActionDiary("1test/02", new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>
            {
                new AllArrearsActionsForTenancy.ArrearsActionDiaryEntry
                {
                    Balance = "10.10",
                    Code = "ABC01",
                    CodeName = "Some Code Name",
                    Date = "11/10/1000",
                    Comment = "Something very interesting!",
                    UniversalHousingUsername = "Vlad"
                },
                new AllArrearsActionsForTenancy.ArrearsActionDiaryEntry
                {
                    Balance = "11.20",
                    Code = "DEF12",
                    CodeName = "Another Code here",
                    Date = "22/08/2000",
                    Comment = "Something very not interesting!",
                    UniversalHousingUsername = "Vlad"
                }
             });

            var response = await GetArrearsActionsDetails(allActions, "1test/02");

            var first = new Dictionary<string, object>
            {
                {"balance", "10.10"},
                {"code", "ABC01"},
                {"code_name", "Some Code Name"},
                {"date", "11/10/1000"},
                {"comment", "Something very interesting!"},
                {"universal_housing_username", "Vlad"}
            };

            var second = new Dictionary<string, object>
            {
                {"balance", "11.20"},
                {"code", "DEF12"},
                {"code_name", "Another Code here"},
                {"date", "22/08/2000"},
                {"comment", "Something very not interesting!"},
                {"universal_housing_username", "Vlad"}
            };

            var output = new Dictionary<string, object>
            {
                {"arrears_action_diary_events",
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
            var controller = new TenancyController(listPaymentsUseCase);
            var result = await controller.PaymentTransactionDetails(tenancyRef);
            return result as OkObjectResult;
        }

        private static async Task<ObjectResult> GetArrearsActionsDetails(IListAllArrearsActions listActionDiaryUseCase,
                                                                         string tenancyRef)
        {
            var controller = new TenancyController(listActionDiaryUseCase);
            var result = await controller.GetActionDiaryDetails(tenancyRef);
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

        private class AllActionDiarySpy : IListAllArrearsActions
        {
            private readonly List<object> calledWith;

            public AllActionDiarySpy()
            {
                calledWith = new List<object>();
            }

            public AllArrearsActionsForTenancy.ArrearsActionDiaryResponse Execute(string tenancyRef)
            {
                calledWith.Add(tenancyRef);
                return new AllArrearsActionsForTenancy.ArrearsActionDiaryResponse {ActionDiaryEntries = new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>()};
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

        private class AllActionsStub : IListAllArrearsActions
        {
            private readonly Dictionary<string, List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>> stubActionDiaryDetails;

            public AllActionsStub()
            {
                stubActionDiaryDetails = new Dictionary<string, List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>>();
            }

            public void AddActionDiary(string tenancyRef, List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry> actionDiary)
            {
                stubActionDiaryDetails[tenancyRef] = actionDiary;
            }

            public AllArrearsActionsForTenancy.ArrearsActionDiaryResponse Execute(string tenancyRef)
            {
                var savedActions = new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>();

                if (stubActionDiaryDetails.ContainsKey(tenancyRef))
                {
                    savedActions = stubActionDiaryDetails[tenancyRef];
                }

                return new AllArrearsActionsForTenancy.ArrearsActionDiaryResponse
                {
                    ActionDiaryEntries = savedActions
                };
            }
        }
    }
}
