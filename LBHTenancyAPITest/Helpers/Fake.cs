using System;
using System.Collections.Generic;
using AgreementService;
using Bogus;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.UseCases;

namespace LBHTenancyAPITest.Helpers
{
    public static class Fake
    {
        public static TenancyListItem GenerateTenancyListItem()
        {
            var random = new Faker();

            return new TenancyListItem
            {
                TenancyRef = random.Random.Hash(11),
                CurrentBalance = random.Finance.Amount(),
                LastActionDate = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                LastActionCode = random.Random.Hash(3),
                ArrearsAgreementStatus = random.Random.Hash(10),
                ArrearsAgreementStartDate =
                  new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                PrimaryContactName = random.Name.FullName(),
                PrimaryContactShortAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
                PrimaryContactPostcode = random.Random.Hash(10)
            };
        }

        public static PaymentTransaction GeneratePaymentTransactionDetails()
        {
            var random = new Faker();

            return new PaymentTransaction
            {
                TenancyRef = random.Random.Hash(11),
                Amount = random.Finance.Amount(),
                Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                TransactionRef = random.Random.Hash(11),
                Type = random.Random.Hash(11),
                PropertyRef = random.Random.Hash(11)
            };
        }

        public static Tenancy GenerateTenancyDetails()
        {
            var random = new Faker();

            return new Tenancy
            {
                TenancyRef = random.Random.Hash(11),
                CurrentBalance = random.Finance.Amount(),
                PrimaryContactName = random.Name.FullName(),
                PrimaryContactLongAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
                PrimaryContactPostcode = random.Random.Hash(10),
                AgreementStatus = random.Random.Hash(10),

                ArrearsActionDiary = new List<ArrearsActionDiaryEntry>
                {
                    new ArrearsActionDiaryEntry()
                    {
                        Balance = random.Finance.Amount(),
                        Code = random.Random.Hash(3),
                        Type = random.Random.Hash(50),
                        Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                        Comment = random.Random.Hash(100),
                        UniversalHousingUsername = random.Random.Hash(50)
                    },
                    new ArrearsActionDiaryEntry()
                    {
                        Balance = random.Finance.Amount(),
                        Code = random.Random.Hash(3),
                        Type = random.Random.Hash(50),
                        Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                        Comment = random.Random.Hash(100),
                        UniversalHousingUsername = random.Random.Hash(50)
                     }
                },
                ArrearsAgreements= new List<ArrearsAgreement>
                {
                   new ArrearsAgreement()
                   {
                       Amount = random.Finance.Amount(),
                       Breached= random.Random.Bool(),
                       Frequency = random.Random.Hash(10),
                       ClearBy = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                       StartBalance = random.Finance.Amount(),
                       Status = random.Random.Hash(10)
                   },
                    new ArrearsAgreement()
                    {
                        Amount = random.Finance.Amount(),
                        Breached= random.Random.Bool(),
                        Frequency = random.Random.Hash(10),
                        ClearBy = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                        StartBalance = random.Finance.Amount(),
                        Status = random.Random.Hash(10)
                    }
                }
            };
        }

        public static ArrearsActionDiaryEntry GenerateActionDiary()
        {
            var random = new Faker();

            return new ArrearsActionDiaryEntry
            {
                TenancyRef = random.Random.Hash(11),
                Balance = random.Finance.Amount(),
                Code = random.Random.Hash(3),
                Type = random.Random.Hash(50),
                Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                Comment = random.Random.Hash(100),
                UniversalHousingUsername = random.Random.Hash(50)
            };
        }

        public static ArrearsActionCreateRequest GenerateActionDiaryRequest()
        {
            var random = new Faker();

            return new ArrearsActionCreateRequest
            {
                ArrearsAction = new ArrearsActionInfo
                {
                    TenancyAgreementRef = random.Random.Hash(11),
                    ActionBalance = random.Finance.Amount(),
                    ActionCode = random.Random.Hash(3),
                    ActionCategory = random.Random.Hash(10),
                    Comment = random.Random.Hash(100)
                }
            };
        }

        public static ArrearsActionResponse CreateArrearsActionAsync(AgreementService.ArrearsActionCreateRequest request)
        {
            var random = new Faker();

            return new ArrearsActionResponse
            {
                ArrearsAction = new ArrearsActionLogDto
                {
                    TenancyAgreementRef = request.ArrearsAction.TenancyAgreementRef,
                    ActionBalance = random.Finance.Amount(),
                    ActionCode = random.Random.Hash(3),
                    ActionCategory = random.Random.Hash(10),
                    //Comment = random.Random.Hash(100)
                },
                ErrorCode = 100,
                ErrorMessage = "",
                Success = true
            };
        }
    }
}
