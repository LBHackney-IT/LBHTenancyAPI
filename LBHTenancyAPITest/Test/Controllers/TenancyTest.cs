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
            allPayments.AddPaymentTransaction(new AllPaymentsForTenancy.PaymentTransaction()
            {
                TenancyRef = "000002/01",
                PropertyRef = "000002/01/11",
                TransactionAmount = "23.01",
                TransactionDate = "2018-01-01 00:00:00Z",
                TransactionType = "Direct Debit",
                TransactionRef = "12345678"
            });
            allPayments.AddPaymentTransaction(new AllPaymentsForTenancy.PaymentTransaction()
            {
                TenancyRef = "000002/01",
                PropertyRef = "000002/02/12",
                TransactionAmount = "24.01",
                TransactionDate = "2018-01-03 00:00:00Z",
                TransactionType = "Direct Debit",
                TransactionRef = "123456789"
            });

            var response = await GetPaymentTransactionDetails(allPayments, "000002/01");

            var first = new Dictionary<string, object>
            {
                {"transaction_ref", "12345678"},
                {"transaction_amount", "23.01"},
                {"transaction_date", "2018-01-01 00:00:00Z"},
                {"transaction_type", "Direct Debit"},
                {"tenancy_ref", "000002/01"},
                {"property_ref", "000002/01/11"}
            };

            var second = new Dictionary<string, object>
            {
                {"transaction_ref", "123456789"},
                {"transaction_amount", "24.01"},
                {"transaction_date", "2018-01-03 00:00:00Z"},
                {"transaction_type", "Direct Debit"},
                {"tenancy_ref", "000002/01"},
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
                new Dictionary<string, object> {{"arrears_action_diary", new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>()}}
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

            allActions.AddActionDiary(new AllArrearsActionsForTenancy.ArrearsActionDiaryEntry()
            {
                ActionBalance = "10.10",
                ActionCode = "ABC01",
                ActionCodeName = "Some Code Name",
                ActionDate = "11/10/1000",
                ActionComment = "Something very interesting!",
                UniversalHousingUsername = "Vlad",
                TenancyRef = "000003/01"
            });
            allActions.AddActionDiary(new AllArrearsActionsForTenancy.ArrearsActionDiaryEntry()
            {
                ActionBalance = "11.20",
                ActionCode = "DEF12",
                ActionCodeName = "Another Code here",
                ActionDate = "22/08/2000",
                ActionComment = "Something very not interesting!",
                UniversalHousingUsername = "Vlad",
                TenancyRef = "000003/01"
            });

            var response = await GetArrearsActionsDetails(allActions, "000003/01");

            var first = new Dictionary<string, object>
            {
                {"action_balance", "10.10"},
                {"action_code", "ABC01"},
                {"action_code_name", "Some Code Name"},
                {"action_date", "11/10/1000"},
                {"action_comment", "Something very interesting!"},
                {"universal_housing_username", "Vlad"},
                {"tenancy_ref", "000003/01"}
            };

            var second = new Dictionary<string, object>
            {
                {"action_balance", "11.20"},
                {"action_code", "DEF12"},
                {"action_code_name", "Another Code here"},
                {"action_date", "22/08/2000"},
                {"action_comment", "Something very not interesting!"},
                {"universal_housing_username", "Vlad"},
                {"tenancy_ref", "000003/01"}
            };

            var output = new Dictionary<string, object>
            {
                {"arrears_action_diary",
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
            private readonly List<AllPaymentsForTenancy.PaymentTransaction> stubPaymentsTransactionsDetails;

            public AllPaymentsStub()
            {
                stubPaymentsTransactionsDetails = new List<AllPaymentsForTenancy.PaymentTransaction>();
            }

            public void AddPaymentTransaction(AllPaymentsForTenancy.PaymentTransaction paymentTransaction)
            {
                stubPaymentsTransactionsDetails.Add(paymentTransaction);
            }

            public AllPaymentsForTenancy.PaymentTransactionResponse Execute(string tenancyRef)
            {

                return new AllPaymentsForTenancy.PaymentTransactionResponse
                {
                    PaymentTransactions = stubPaymentsTransactionsDetails.FindAll(e => e.TenancyRef == tenancyRef)
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
            private readonly List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry> stubActionDiaryDetails;

            public AllActionsStub()
            {
                stubActionDiaryDetails = new List<AllArrearsActionsForTenancy.ArrearsActionDiaryEntry>();
            }

            public void AddActionDiary(AllArrearsActionsForTenancy.ArrearsActionDiaryEntry actionDiary)
            {
                stubActionDiaryDetails.Add(actionDiary);
            }

            public AllArrearsActionsForTenancy.ArrearsActionDiaryResponse Execute(string tenancyRef)
            {

                return new AllArrearsActionsForTenancy.ArrearsActionDiaryResponse
                {
                    ActionDiaryEntries = stubActionDiaryDetails.FindAll(e => e.TenancyRef == tenancyRef)
                };
            }
        }
    }
}
