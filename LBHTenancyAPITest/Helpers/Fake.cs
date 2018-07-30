using System;
using Bogus;
using LBHTenancyAPI.Domain;

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
              PrimaryContactLongAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
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

        public static ArrearsActionDiaryEntry GenerateActionDiary()
        {
            var random = new Faker();

            return new ArrearsActionDiaryEntry
            {
                TenancyRef = random.Random.Hash(11),
                Balance = random.Finance.Amount(),
                Code = random.Random.Hash(3),
                CodeName = random.Random.Hash(50),
                Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                Comment = random.Random.Hash(100),
                UniversalHousingUsername = random.Random.Hash(50)
            };
        }
    }
}
