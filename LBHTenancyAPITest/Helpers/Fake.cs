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
              PrimaryContactShortAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
              PrimaryContactPostcode = random.Random.Hash(10)
          };
        }

        public static PaymentTransactionDetails GeneratePaymentTransactionDetails()
        {
            var random = new Faker();

            return new PaymentTransactionDetails
            {
                TenancyRef = random.Random.Hash(11),
                TransactionAmount = random.Finance.Amount(),
                TransactionDate = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                TransactionsRef = random.Random.Hash(11),
                TransactionType = random.Random.Hash(11),
                PropertyRef = random.Random.Hash(11)
            };
        }

        public static ArrearsActionDiaryDetails GenerateActionDiary()
        {
            var random = new Faker();

            return new ArrearsActionDiaryDetails
            {
                TenancyRef = random.Random.Hash(11),
                ActionBalance = random.Finance.Amount(),
                ActionCode = random.Random.Hash(3),
                ActionCodeName = random.Random.Hash(50),
                ActionDate = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                ActionComment = random.Random.Hash(100),
                UniversalHousingUsername = random.Random.Hash(50)
            };
        }
    }
}
