using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bogus;
using Dapper;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways
{
    public class UhTenanciesGatewayTest : IClassFixture<DatabaseFixture>
    {
        private readonly SqlConnection db;
        private static readonly TimeSpan DAY_IN_TIMESPAN = new TimeSpan(1, 0, 0, 0);

        public UhTenanciesGatewayTest(DatabaseFixture fixture)
        {
            db = fixture.Db;
        }

        [Fact]
        public void WhenGivenNoTenancyRefs_GetTenanciesByRefs_ShouldReturnNoTenancies()
        {
            var tenancies = GetTenanciesByRef(new List<string>());

            Assert.Empty(tenancies);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetTenanciesByRefs_ShouldReturnTenancyObjectForThatRef()
        {
            TenancyListItem expectedTenancy = InsertRandomisedTenancyListItem();

            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});

            Assert.Single(tenancies);
            Assert.Contains(expectedTenancy, tenancies);
        }

        [Fact]
        public void WhenGivenSomeTenancyRefs_GetTenanciesByRefs_ShouldReturnTenancyObjectForEachValidRef()
        {
            TenancyListItem expectedTenancy1 = InsertRandomisedTenancyListItem();
            TenancyListItem expectedTenancy2 = InsertRandomisedTenancyListItem();

            var tenancies = GetTenanciesByRef(new List<string>
            {
                expectedTenancy1.TenancyRef,
                "NotValid",
                expectedTenancy2.TenancyRef,
                "NotPresent"
            });

            Assert.Equal(2, tenancies.Count);
            Assert.Contains(expectedTenancy1, tenancies);
            Assert.Contains(expectedTenancy2, tenancies);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetTenanciesByRefs_ShouldReturnTheLatestAgreement()
        {
            TenancyListItem expectedTenancy = InsertRandomisedTenancyListItem();

            DateTime latestAragDate = expectedTenancy.ArrearsAgreementStartDate.AddDays(1);
            InsertAgreement(expectedTenancy.TenancyRef, "Inactive",expectedTenancy.ArrearsAgreementStartDate.Subtract(DAY_IN_TIMESPAN));
            InsertAgreement(expectedTenancy.TenancyRef, "Active", latestAragDate);

            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});
            Assert.Equal(tenancies[0].ArrearsAgreementStartDate, latestAragDate);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetTenanciesByRefs_ShouldReturnTheLatestArrearsAction()
        {
            TenancyListItem expectedTenancy = InsertRandomisedTenancyListItem();

            DateTime latestActionDate = expectedTenancy.LastActionDate.AddDays(1);
            InsertArrearsActions(expectedTenancy.TenancyRef, "ABC",
                expectedTenancy.LastActionDate.Subtract(DAY_IN_TIMESPAN));
            InsertArrearsActions(expectedTenancy.TenancyRef, "XYZ", latestActionDate);

            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});
            Assert.Equal(tenancies[0].LastActionDate, latestActionDate);
        }

        [Fact]
        public void WhenGivenAListOfTenancyRefs_GetTenanciesByRefs_ShouldReturnAllUniqueTenancies()
        {
            TenancyListItem firstTenancy = InsertRandomisedTenancyListItem();
            TenancyListItem secondTenancy = InsertRandomisedTenancyListItem();

            DateTime firstTenancyLatestActionDate = firstTenancy.LastActionDate.AddDays(1);
            InsertArrearsActions(firstTenancy.TenancyRef, "ABC", firstTenancyLatestActionDate);

            DateTime secondTenancyLatestAgreementStartDate = secondTenancy.ArrearsAgreementStartDate.AddDays(1);
            InsertAgreement(secondTenancy.TenancyRef, "characters", secondTenancyLatestAgreementStartDate);

            var tenancies = GetTenanciesByRef(new List<string> {firstTenancy.TenancyRef, secondTenancy.TenancyRef});

            var receivedFirst = tenancies.Find(e => e.TenancyRef == firstTenancy.TenancyRef);
            Assert.Equal(firstTenancyLatestActionDate, receivedFirst.LastActionDate);
            Assert.Equal("ABC", receivedFirst.LastActionCode);

            var receivedSecond = tenancies.Find(e => e.TenancyRef == secondTenancy.TenancyRef);
            Assert.Equal(secondTenancyLatestAgreementStartDate, receivedSecond.ArrearsAgreementStartDate);
            Assert.Equal("characters", receivedSecond.ArrearsAgreementStatus);
        }

        [Fact]
        public void WhenGivenAListOfTenancyRefs_GetTenanciesByRefs_ShouldTrimCharacterFields()
        {
            string commandText =
                "INSERT INTO tenagree (tag_ref) VALUES (@tenancyRef);" +
                "INSERT INTO araction (tag_ref, action_code) VALUES (@tenancyRef, @actionCode)" +
                "INSERT INTO arag (tag_ref, arag_status) VALUES (@tenancyRef, @aragStatus)" +
                "INSERT INTO contacts (tag_ref, con_postcode, con_phone1) VALUES (@tenancyRef, @postcode, @phone)";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = "not11chars";
            command.Parameters.Add("@actionCode", SqlDbType.Char);
            command.Parameters["@actionCode"].Value = "ee";
            command.Parameters.Add("@aragStatus", SqlDbType.Char);
            command.Parameters["@aragStatus"].Value = "status";
            command.Parameters.Add("@postcode", SqlDbType.Char);
            command.Parameters["@postcode"].Value = "pcode";
            command.Parameters.Add("@phone", SqlDbType.Char);
            command.Parameters["@phone"].Value = "phone";

            command.ExecuteNonQuery();

            string retrieved_value = db.Query<string>("SELECT TOP 1 tag_ref FROM tenagree WHERE tag_ref = 'not11chars '").First();
            Assert.Contains("not11chars ", retrieved_value);

            retrieved_value = db.Query<string>("SELECT TOP 1 action_code FROM araction WHERE tag_ref = 'not11chars '").First();
            Assert.Contains("ee ", retrieved_value);

            retrieved_value = db.Query<string>("SELECT TOP 1 arag_status FROM arag WHERE tag_ref = 'not11chars '").First();
            Assert.Contains("status    ", retrieved_value);

            List<dynamic> retrieved_values = db.Query("SELECT tag_ref, con_postcode, con_phone1 FROM contacts WHERE contacts.tag_ref = 'not11chars '").ToList();
            IDictionary<string, object> row = retrieved_values[0];

            Assert.Contains("pcode     ", row.Values);
            Assert.Contains("phone                ", row.Values);


            TenancyListItem trimmedTenancy = GetTenanciesByRef(new List<string> {"not11chars"}).First();

            Assert.Equal("not11chars", trimmedTenancy.TenancyRef);
            Assert.Equal("ee", trimmedTenancy.LastActionCode);
            Assert.Equal("status", trimmedTenancy.ArrearsAgreementStatus);
            Assert.Equal("pcode", trimmedTenancy.PrimaryContactPostcode);
        }

        [Fact]
        public void WhenGivenATenancyRef_GetTenanciesByRefs_ShouldReturnOnlyTheShortAddress()
        {
            var random = new Randomizer();

            TenancyListItem expectedTenancy = InsertRandomisedTenancyListItem();

            string longAddress = $"{expectedTenancy.PrimaryContactShortAddress}\n" +
                                 $"{random.Words()}\n{random.Words()}\n{random.Words()}";

            // make sure there's a long string in the db
            string commandText =
                $"UPDATE contacts SET con_address = '{longAddress}' WHERE contacts.tag_ref = '{expectedTenancy.TenancyRef}'";
            SqlCommand command = new SqlCommand(commandText, db);
            command.ExecuteNonQuery();

            string actualShortAddressExpected = longAddress.Split("\n")[0];
            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});

            Assert.Equal(actualShortAddressExpected, tenancies[0].PrimaryContactShortAddress);
            Assert.NotEqual(longAddress, tenancies[0].PrimaryContactShortAddress);
        }

        [Fact]
        public void WhenGivenATenancyRefWithNoAddress_GetTenanciesByRefs_ShouldReturnNull()
        {
            TenancyListItem expectedTenancy = CreateRandomTenancyListItem();
            expectedTenancy.PrimaryContactShortAddress = null;
            InsertTenancyAttributes(expectedTenancy);

            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});

            Assert.Equal(expectedTenancy.PrimaryContactShortAddress, tenancies[0].PrimaryContactShortAddress);
        }

        [Fact]
        public void WhenGivenNoTenancyRefs_GetSingleTenancyByRefs_ShouldReturnEmptyTenancy()
        {
            var tenancy = GetSingleTenacyForRef("i_do_not_exist");

            Assert.Null(tenancy.TenancyRef);
        }


        [Fact]
        public void WhenGivenTenancyRefThatisUrlEncoded_GetSingleTenancyByRefs_ShouldRespondWithResults()
        {
            Tenancy expectedTenancy = CreateRandomSingleTenancyItem();
            expectedTenancy.TenancyRef = "Test/01";
            InsertSingleTenancyAttributes(expectedTenancy);

            var tenancy = GetSingleTenacyForRef("Test%2F01");

            Assert.Equal("Test/01", tenancy.TenancyRef);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetSingleTenancyByRef_ShouldReturnTenancyWithBasicDetails()
        {
            Tenancy expectedTenancy = CreateRandomSingleTenancyItem();
            InsertSingleTenancyAttributes(expectedTenancy);

            var tenancy = GetSingleTenacyForRef(expectedTenancy.TenancyRef);

            Assert.Equal(expectedTenancy.PrimaryContactName, tenancy.PrimaryContactName);
            Assert.Equal(expectedTenancy.PrimaryContactPostcode, tenancy.PrimaryContactPostcode);
            Assert.Equal(expectedTenancy.PrimaryContactLongAddress, tenancy.PrimaryContactLongAddress);
            Assert.Equal(expectedTenancy.PrimaryContactPhone, tenancy.PrimaryContactPhone);
            Assert.Equal(expectedTenancy.CurrentBalance, tenancy.CurrentBalance);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetSingleTenancyByRef_ShouldReturnTenancyWithLatestFiveArrearsActions()
        {
            Tenancy expectedTenancy= CreateRandomSingleTenancyItem();
            InsertSingleTenancyAttributes(expectedTenancy);

            expectedTenancy.ArrearsActionDiary = InsertRandomActionDiaryDetails(expectedTenancy.TenancyRef, 6);

            var tenancy = GetSingleTenacyForRef(expectedTenancy.TenancyRef);
            Assert.Equal(expectedTenancy.PrimaryContactName, tenancy.PrimaryContactName);
            Assert.Equal(expectedTenancy.PrimaryContactPostcode, tenancy.PrimaryContactPostcode);
            Assert.Equal(expectedTenancy.PrimaryContactLongAddress, tenancy.PrimaryContactLongAddress);
            Assert.Equal(expectedTenancy.PrimaryContactPhone, tenancy.PrimaryContactPhone);

            Assert.Equal(5, tenancy.ArrearsActionDiary.Count);
            Assert.True(tenancy.ArrearsActionDiary[0].Date.Ticks >= tenancy.ArrearsActionDiary[1].Date.Ticks);

            var oldestDate = expectedTenancy.ArrearsActionDiary.OrderBy(d => d.Date).First();
            Assert.True(tenancy.ArrearsActionDiary[4].Date.Ticks >= oldestDate.Date.Ticks);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetSingleTenancyByRef_ShouldReturnTenancyWithLatestFiveAgreements()
        {
            Tenancy expectedTenancy= CreateRandomSingleTenancyItem();
            InsertSingleTenancyAttributes(expectedTenancy);

            expectedTenancy.ArrearsAgreements = InsertRandomAgreementDetails(expectedTenancy.TenancyRef, 6);

            var tenancy = GetSingleTenacyForRef(expectedTenancy.TenancyRef);
            Assert.Equal(expectedTenancy.PrimaryContactName, tenancy.PrimaryContactName);
            Assert.Equal(expectedTenancy.PrimaryContactPostcode, tenancy.PrimaryContactPostcode);
            Assert.Equal(expectedTenancy.PrimaryContactLongAddress, tenancy.PrimaryContactLongAddress);
            Assert.Equal(expectedTenancy.PrimaryContactPhone, tenancy.PrimaryContactPhone);

            Assert.Equal(5, tenancy.ArrearsAgreements.Count);
            Assert.True(tenancy.ArrearsAgreements[0].Startdate.Ticks >= tenancy.ArrearsAgreements[1].Startdate.Ticks);

            var oldestDate = expectedTenancy.ArrearsAgreements.OrderBy(d => d.Startdate).First();
            Assert.True(tenancy.ArrearsAgreements[4].Startdate.Ticks >= oldestDate.Startdate.Ticks);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetActionDiaryDetailsbyTenancyRef_ShouldReturnAllArrearsActions()
        {
            int numberOfExpectedActions = 10;
            string expectedTenancyRef = "12345/01";

            InsertRandomActionDiaryDetails(expectedTenancyRef, numberOfExpectedActions);

            var actions = GetArrearsActionsByRef(expectedTenancyRef);

            Assert.Equal(numberOfExpectedActions, actions.Count);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetPaymentTransactionsByTenancyRef_ShouldReturnAllPayments()
        {
            int numberOfExpectedTransactions = 10;
            string expectedTenancyRef = "12345/01";

            InsertRandomTransactions(expectedTenancyRef, numberOfExpectedTransactions);

            var transactions = GetPaymentTransactionsByTenancyRef(expectedTenancyRef);

            Assert.Equal(numberOfExpectedTransactions, transactions.Count);
        }

        private Tenancy GetSingleTenacyForRef(string tenancyRef)
        {
            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            return gateway.GetTenancyForRef(tenancyRef);
        }

        private List<TenancyListItem> GetTenanciesByRef(List<string> refs)
        {
            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            return gateway.GetTenanciesByRefs(refs);
        }

        private List<ArrearsActionDiaryEntry> GetArrearsActionsByRef(string tenancyRef)
        {
            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            return gateway.GetActionDiaryEntriesbyTenancyRef(tenancyRef);
        }

        private List<PaymentTransaction> GetPaymentTransactionsByTenancyRef(string tenancyRef)
        {
            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            return gateway.GetPaymentTransactionsByTenancyRef(tenancyRef);
        }

        private Tenancy CreateRandomSingleTenancyItem()
        {
            var random = new Faker();
            return new Tenancy
            {
                TenancyRef = random.Random.Hash(11),
                CurrentBalance = random.Finance.Amount(),
                PrimaryContactName = random.Name.FullName(),
                PrimaryContactLongAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
                PrimaryContactPostcode = random.Random.Hash(10),
                PrimaryContactPhone = random.Random.Hash(21),
                ArrearsAgreements = new List<ArrearsAgreement>()
            };
        }

        private TenancyListItem CreateRandomTenancyListItem()
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

        private TenancyListItem InsertRandomisedTenancyListItem()
        {
            TenancyListItem tenancy = CreateRandomTenancyListItem();
            InsertTenancyAttributes(tenancy);

            return tenancy;
        }

        private string InsertQueries()
        {
            string commandText =
                "INSERT INTO tenagree (tag_ref, cur_bal) VALUES (@tenancyRef, @currentBalance);" +
                "INSERT INTO contacts (tag_ref, con_name, con_address, con_postcode, con_phone1) VALUES (@tenancyRef, @primaryContactName, @primaryContactAddress, @primaryContactPostcode, @primaryContactPhone);";

            return commandText;
        }

        private void InsertTenancyAttributes(TenancyListItem tenancyAttributes)
        {
            string commandText = InsertQueries();
            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = tenancyAttributes.TenancyRef;
            command.Parameters.Add("@currentBalance", SqlDbType.Decimal);
            command.Parameters["@currentBalance"].Value = tenancyAttributes.CurrentBalance;
            command.Parameters.Add("@primaryContactName", SqlDbType.Char);
            command.Parameters["@primaryContactName"].Value = tenancyAttributes.PrimaryContactName;
            command.Parameters.Add("@primaryContactAddress", SqlDbType.Char);
            command.Parameters["@primaryContactAddress"].Value =
                tenancyAttributes.PrimaryContactShortAddress == null
                    ? DBNull.Value.ToString()
                    : tenancyAttributes.PrimaryContactShortAddress + "\n";
            command.Parameters.Add("@primaryContactPostcode", SqlDbType.Char);
            command.Parameters["@primaryContactPostcode"].Value = tenancyAttributes.PrimaryContactPostcode;
            command.Parameters.Add("@primaryContactPhone", SqlDbType.Char);
            command.Parameters["@primaryContactPhone"].Value = DBNull.Value.ToString();
            command.ExecuteNonQuery();

            InsertAgreement(tenancyAttributes.TenancyRef, tenancyAttributes.ArrearsAgreementStatus,tenancyAttributes.ArrearsAgreementStartDate);
            InsertArrearsActions(tenancyAttributes.TenancyRef, tenancyAttributes.LastActionCode,tenancyAttributes.LastActionDate);
        }

        private void InsertSingleTenancyAttributes(Tenancy tenancyValues)
        {
            string commandText = InsertQueries();
            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = tenancyValues.TenancyRef;
            command.Parameters.Add("@currentBalance", SqlDbType.Decimal);
            command.Parameters["@currentBalance"].Value = tenancyValues.CurrentBalance;
            command.Parameters.Add("@primaryContactName", SqlDbType.Char);
            command.Parameters["@primaryContactName"].Value = tenancyValues.PrimaryContactName;
            command.Parameters.Add("@primaryContactAddress", SqlDbType.Char);
            command.Parameters["@primaryContactAddress"].Value =
                tenancyValues.PrimaryContactLongAddress == null
                    ? DBNull.Value.ToString()
                    : tenancyValues.PrimaryContactLongAddress + "\n";
            command.Parameters.Add("@primaryContactPostcode", SqlDbType.Char);
            command.Parameters["@primaryContactPostcode"].Value = tenancyValues.PrimaryContactPostcode;
            command.Parameters.Add("@primaryContactPhone", SqlDbType.Char);
            command.Parameters["@primaryContactPhone"].Value = tenancyValues.PrimaryContactPhone;
            command.ExecuteNonQuery();
        }

        private void InsertAgreement(string tenancyRef, string status, DateTime startDate)
        {
            string commandText =
                "INSERT INTO arag (tag_ref, arag_status, arag_startdate) VALUES (@tenancyRef, @agreementStatus, @startDate)";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = tenancyRef;
            command.Parameters.Add("@agreementStatus", SqlDbType.Char);
            command.Parameters["@agreementStatus"].Value = status;
            command.Parameters.Add("@startDate", SqlDbType.SmallDateTime);
            command.Parameters["@startDate"].Value = startDate;

            command.ExecuteNonQuery();
        }

        private List<ArrearsAgreement> InsertRandomAgreementDetails(string tenancyRef, int num)
        {
            var random = new Faker();
            List<ArrearsAgreement> items = new List<ArrearsAgreement>();

            string commandText =
                "INSERT INTO arag (tag_ref, arag_status, arag_startdate, arag_amount, arag_startbal, arag_frequency, arag_breached, arag_clearby) " +
                "VALUES (@tenancyRef, @agreementStatus, @startDate, @amount, @startBal, @frequency, @breached, @clearBy)";

            foreach (int i in Enumerable.Range(0, num))
            {
                ArrearsAgreement arrearsAgreement = new ArrearsAgreement
                {
                    Amount = random.Finance.Amount(),
                    Breached = true,
                    ClearBy = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                    Frequency = $"FR{i}",
                    StartBalance = random.Finance.Amount(),
                    Startdate = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                    Status = $"AB{i}",
                    TenancyRef = tenancyRef
                };

                SqlCommand command = new SqlCommand(commandText, db);
                command.Parameters.Add("@tenancyRef", SqlDbType.Char);
                command.Parameters["@tenancyRef"].Value = arrearsAgreement.TenancyRef;
                command.Parameters.Add("@agreementStatus", SqlDbType.Char);
                command.Parameters["@agreementStatus"].Value = arrearsAgreement.Status;
                command.Parameters.Add("@startDate", SqlDbType.SmallDateTime);
                command.Parameters["@startDate"].Value = arrearsAgreement.Startdate;
                command.Parameters.Add("@amount", SqlDbType.Decimal);
                command.Parameters["@amount"].Value = arrearsAgreement.Amount;
                command.Parameters.Add("@startBal", SqlDbType.Decimal);
                command.Parameters["@startBal"].Value = arrearsAgreement.StartBalance;
                command.Parameters.Add("@frequency", SqlDbType.Char);
                command.Parameters["@frequency"].Value = arrearsAgreement.Frequency;
                command.Parameters.Add("@breached", SqlDbType.Bit);
                command.Parameters["@breached"].Value = 1;
                command.Parameters.Add("@clearBy", SqlDbType.SmallDateTime);
                command.Parameters["@clearBy"].Value = arrearsAgreement.ClearBy;

                items.Add(arrearsAgreement);
                command.ExecuteNonQuery();
            }

            return items.OrderByDescending(i => i.Startdate).ToList();
        }


        private List<ArrearsActionDiaryEntry> InsertRandomActionDiaryDetails(string tenancyRef, int num)
        {
            List<ArrearsActionDiaryEntry> items = null;
            SqlCommand command = null;
            try
            {
                var random = new Faker();
                items = new List<ArrearsActionDiaryEntry>();
                string commandText =

                    "INSERT INTO araction (tag_ref, action_code, action_type, action_date, action_comment, action_balance, " +
                    "username) " +
                    "VALUES (@tenancyRef, @actionCode, @actionType, @actionDate, @actionComment, @actionBalance, @uhUsername)";

                foreach (int i in Enumerable.Range(0, num))
                {
                     ArrearsActionDiaryEntry arrearsActionDiaryEntry = new ArrearsActionDiaryEntry
                     {
                        TenancyRef = tenancyRef,
                        Code = random.Random.Hash(3),
                        Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
                        Type = random.Random.Hash(3),
                        Comment = random.Random.Hash(50),
                        Balance = random.Finance.Amount(),
                        UniversalHousingUsername = random.Random.Hash(40)
                    };

                    command = new SqlCommand(commandText, db);
                    command.Parameters.Add("@tenancyRef", SqlDbType.Char);
                    command.Parameters["@tenancyRef"].Value = arrearsActionDiaryEntry.TenancyRef;

                    command.Parameters.Add("@actionCode", SqlDbType.Char);
                    command.Parameters["@actionCode"].Value = arrearsActionDiaryEntry.Code;

                    command.Parameters.Add("@actionDate", SqlDbType.SmallDateTime);
                    command.Parameters["@actionDate"].Value = arrearsActionDiaryEntry.Date;

                    command.Parameters.Add("@actionType", SqlDbType.Char);
                    command.Parameters["@actionType"].Value = arrearsActionDiaryEntry.Type;

                    command.Parameters.Add("@actionComment", SqlDbType.NVarChar);
                    command.Parameters["@actionComment"].Value = arrearsActionDiaryEntry.Comment;

                    command.Parameters.Add("@actionBalance", SqlDbType.Decimal);
                    command.Parameters["@actionBalance"].Value = arrearsActionDiaryEntry.Balance;

                    command.Parameters.Add("@uhUsername", SqlDbType.Char);
                    command.Parameters["@uhUsername"].Value = arrearsActionDiaryEntry.UniversalHousingUsername;

                    items.Add(arrearsActionDiaryEntry);
                    command.ExecuteNonQuery();
                }

                return items.OrderByDescending(i => i.Date).ToList();
            }
            catch (Exception ex)
            {
               throw ex;
            }
            finally
            {
                command = null;
            }
        }

        private List<PaymentTransaction> InsertRandomTransactions(string tenancyRef, int num)
        {
            List<PaymentTransaction> items = null;
            SqlCommand command = null;
            try
            {
                var random = new Faker();
                items = new List<PaymentTransaction>();
                string commandText =

                    "INSERT INTO rtrans (tag_ref, trans_ref, prop_ref, trans_type, post_date, real_value)" +
                    "VALUES (@tenancyRef, @transRef, @propRef, @transType, @transactionDate, @amount)";

                foreach (int i in Enumerable.Range(0, num))
                {
                     PaymentTransaction payment = new PaymentTransaction
                     {
                        TenancyRef = tenancyRef,
                        Type = random.Random.Hash(3),
                        PropertyRef = random.Random.Hash(12),
                        TransactionRef= random.Random.Hash(12),
                        Amount = random.Finance.Amount(),
                        Date = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0)
                    };

                    command = new SqlCommand(commandText, db);
                    command.Parameters.Add("@tenancyRef", SqlDbType.Char);
                    command.Parameters["@tenancyRef"].Value = payment.TenancyRef;

                    command.Parameters.Add("@transRef", SqlDbType.Char);
                    command.Parameters["@transRef"].Value = payment.TransactionRef;

                    command.Parameters.Add("@transactionDate", SqlDbType.SmallDateTime);
                    command.Parameters["@transactionDate"].Value = payment.Date;

                    command.Parameters.Add("@propRef", SqlDbType.Char);
                    command.Parameters["@propRef"].Value = payment.PropertyRef;

                    command.Parameters.Add("@amount", SqlDbType.Decimal);
                    command.Parameters["@amount"].Value = payment.Amount;

                    command.Parameters.Add("@transType", SqlDbType.Char);
                    command.Parameters["@transType"].Value = payment.Type;

                    items.Add(payment);
                    command.ExecuteNonQuery();
                }

                return items.OrderByDescending(i => i.Date).ToList();
            }
            catch (Exception ex)
            {
               throw ex;
            }
            finally
            {
                command = null;
            }
        }


        private void InsertArrearsActions(string tenancyRef, string actionCode, DateTime actionDate)
        {
            string commandText =
                "INSERT INTO araction (tag_ref, action_code, action_date) VALUES (@tenancyRef, @actionCode, @actionDate)";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = tenancyRef;
            command.Parameters.Add("@actionCode", SqlDbType.Char);
            command.Parameters["@actionCode"].Value = actionCode;
            command.Parameters.Add("@actionDate", SqlDbType.SmallDateTime);
            command.Parameters["@actionDate"].Value = actionDate;

            command.ExecuteNonQuery();
        }
    }
}
