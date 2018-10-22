using System;
using System.Collections.Generic;
using Bogus;
using LBH.Data.Domain;
using Bogus.DataSets;
using AgreementService;
using LBHTenancyAPITest.Helpers.Entities;
using ArrearsAgreement = LBHTenancyAPITest.Helpers.Entities.ArrearsAgreement;
using Contact = LBHTenancyAPITest.Helpers.Entities.Contact;

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
              PropertyRef = random.Random.Hash(12),
              Tenure = random.Random.Hash(3),
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
                PropertyRef = random.Random.Hash(11),
                Description = random.Random.String()
            };
        }

        public static Tenancy GenerateTenancyDetails()
        {
            var random = new Faker();

            return new Tenancy
            {
                TenancyRef = random.Random.Hash(11),
                PropertyRef = random.Random.Hash(12),
                Tenure = random.Random.Hash(3),
                CurrentBalance = random.Finance.Amount(),
                Rent = random.Finance.Amount(),
                Service = random.Finance.Amount(),
                OtherCharge = random.Finance.Amount(),
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
                ArrearsAgreements= new List<LBH.Data.Domain.ArrearsAgreement>
                {
                   new LBH.Data.Domain.ArrearsAgreement()
                   {
                       Amount = random.Finance.Amount(),
                       Breached= random.Random.Bool(),
                       Frequency = random.Random.Hash(10),
                       ClearBy = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                       StartBalance = random.Finance.Amount(),
                       Status = random.Random.Hash(10)
                   },
                    new LBH.Data.Domain.ArrearsAgreement()
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

        public static ArrearsActionResponse CreateArrearsActionAsync(ArrearsActionCreateRequest request)
        {
            var random = new Faker();

            return new ArrearsActionResponse
            {
                ArrearsAction = new ArrearsActionLogDto
                {
                    TenancyAgreementRef = request.ArrearsAction.TenancyAgreementRef,
                    ActionBalance = request.ArrearsAction.ActionBalance ?? random.Finance.Amount() ,
                    ActionCode = request.ArrearsAction.ActionCode,
                    ActionCategory = request.ArrearsAction.ActionCategory,
                },
                ErrorCode = 0,
                ErrorMessage = "",
                Success = true
            };
        }

        public static class UniversalHousing
        {


            public static Member GenerateFakeMember()
            {
                var faker = new Faker<Member>()
                    .RuleFor(property => property.house_ref, (fake, model) => fake.Random.AlphaNumeric(10))
                    .RuleFor(property => property.surname, (fake, model) => fake.Name.LastName())
                    .RuleFor(property => property.forename, (fake, model) => fake.Name.FirstName().Trim())
                    .RuleFor(property => property.title, (fake, model) => "Mr")
                    .RuleFor(property => property.age, (fake, model) => fake.Random.Int(20, 50))
                    .RuleFor(property => property.responsible, (fake, model) => true)
                    .RuleFor(property => property.person_no, (fake, model) => fake.IndexFaker)
                    ;
                var member = faker.Generate();
                return member;
            }

            public static TenancyAgreement GenerateFakeTenancy()
            {
                var faker = new Faker<TenancyAgreement>()
                        .RuleFor(property => property.tag_ref, (fake, model) => fake.Random.AlphaNumeric(11))
                        .RuleFor(property => property.prop_ref, (fake, model) => fake.Random.AlphaNumeric(12))
                        .RuleFor(property => property.cur_bal, (fake, model) => fake.Finance.Amount())
                        .RuleFor(property => property.house_ref, (fake, model) => fake.Random.AlphaNumeric(10))
                        .RuleFor(property => property.tenure, (fake, model) => fake.Random.AlphaNumeric(3))
                        .RuleFor(property => property.service, (fake, model) => fake.Finance.Amount())
                        .RuleFor(property => property.rent, (fake, model) => fake.Finance.Amount())
                        .RuleFor(property => property.other_charge, (fake, model) => fake.Finance.Amount())
                    ;

                var faked = faker.Generate();
                return faked;
            }

            public static Property GenerateFakeProperty()
            {
                var faker = new Faker<Property>()
                        
                        .RuleFor(property => property.prop_ref, (fake, model) => fake.Random.AlphaNumeric(12))
                        .RuleFor(property => property.address1, (fake, model) => fake.Address.FullAddress())
                        .RuleFor(property => property.post_code, (fake, model) => fake.Address.ZipCode())
                        .RuleFor(property => property.short_address, (fake, model) => model.address1)
                    ;

                var faked = faker.Generate();
                return faked;
            }

            public static ArrearsAgreement GenerateFakeArrearsAgreement()
            {
                var faker = new Faker<ArrearsAgreement>()

                        .RuleFor(property => property.arag_ref, (fake, model) => fake.Random.AlphaNumeric(14))
                        .RuleFor(property => property.arag_status, (fake, model) => fake.Random.AlphaNumeric(10))
                        .RuleFor(property => property.tag_ref, (fake, model) => fake.Random.AlphaNumeric(11))
                        .RuleFor(property => property.arag_startdate, (fake, model) => DateTime.Now.Date)
                        .RuleFor(property => property.arag_sid, (fake, model) => fake.IndexFaker)
                    ;

                var faked = faker.Generate();
                return faked;
            }

            public static ArrearsAgreementDet GenerateFakeArrearsAgreementDet()
            {
                var faker = new Faker<ArrearsAgreementDet>()
                        .RuleFor(property => property.tag_ref, (fake, model) => fake.Random.AlphaNumeric(11))
                        .RuleFor(property => property.arag_sid, (fake, model) => fake.IndexFaker)
                        .RuleFor(property => property.amount, (fake, model) => fake.Finance.Amount())
                        .RuleFor(property => property.aragdet_frequency, (fake, model) => "1")
                    ;

                var faked = faker.Generate();
                return faked;
            }

            public static Contact GenerateFakeContact()
            {
                var faker = new Faker<Contact>()
                        .RuleFor(property => property.tag_ref, (fake, model) => fake.Random.AlphaNumeric(11))
                        .RuleFor(property => property.con_name, (fake, model) => fake.Name.FullName())
                        .RuleFor(property => property.prop_ref, (fake, model) => fake.Random.AlphaNumeric(12))
                    ;

                var faked = faker.Generate();
                return faked;
            }
        }
    }
}
