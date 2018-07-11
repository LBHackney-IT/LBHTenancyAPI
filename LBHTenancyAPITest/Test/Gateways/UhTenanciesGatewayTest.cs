using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bogus;
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
        public void WhenGivenTenancyRef_GetTenanciesByRefs_ShouldReturnTheLatestAgreement()
        {
            TenancyListItem expectedTenancy = InsertRandomisedTenancyListItem();

            DateTime latestAragDate = expectedTenancy.ArrearsAgreementStartDate.AddDays(1);
            InsertAgreement(expectedTenancy.TenancyRef, "Inactive",
                expectedTenancy.ArrearsAgreementStartDate.Subtract(DAY_IN_TIMESPAN));
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
            InsertAgreement(secondTenancy.TenancyRef, "Activeo", secondTenancyLatestAgreementStartDate);

            var tenancies = GetTenanciesByRef(new List<string> {firstTenancy.TenancyRef, secondTenancy.TenancyRef});

            var receivedFirst = tenancies.Find(e => e.TenancyRef == firstTenancy.TenancyRef);
            Assert.Equal(firstTenancyLatestActionDate, receivedFirst.LastActionDate);
            Assert.Equal("ABC", receivedFirst.LastActionCode);

            var receivedSecond = tenancies.Find(e => e.TenancyRef == secondTenancy.TenancyRef);
            Assert.Equal(secondTenancyLatestAgreementStartDate, receivedSecond.ArrearsAgreementStartDate);
            Assert.Equal("Activeo", receivedSecond.ArrearsAgreementStatus);
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

        private List<TenancyListItem> GetTenanciesByRef(List<string> refs)
        {
            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            var tenancies = gateway.GetTenanciesByRefs(refs);

            return tenancies;
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

        private void InsertTenancyAttributes(TenancyListItem tenancyAttributes)
        {
            string commandText =
                "INSERT INTO tenagree (tag_ref, cur_bal) VALUES (@tenancyRef, @currentBalance);" +
                "INSERT INTO contacts (tag_ref, con_name, con_address, con_postcode) VALUES (@tenancyRef, @primaryContactName, @primaryContactAddress, @primaryContactPostcode);";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = tenancyAttributes.TenancyRef;
            command.Parameters.Add("@currentBalance", SqlDbType.Decimal);
            command.Parameters["@currentBalance"].Value = tenancyAttributes.CurrentBalance;
            command.Parameters.Add("@primaryContactName", SqlDbType.VarChar);
            command.Parameters["@primaryContactName"].Value = tenancyAttributes.PrimaryContactName;
            command.Parameters.Add("@primaryContactAddress", SqlDbType.Char);
            command.Parameters["@primaryContactAddress"].Value =
                tenancyAttributes.PrimaryContactShortAddress == null
                    ? DBNull.Value.ToString()
                    : tenancyAttributes.PrimaryContactShortAddress;
            command.Parameters.Add("@primaryContactPostcode", SqlDbType.Char);
            command.Parameters["@primaryContactPostcode"].Value = tenancyAttributes.PrimaryContactPostcode;

            command.ExecuteNonQuery();

            InsertAgreement(tenancyAttributes.TenancyRef, tenancyAttributes.ArrearsAgreementStatus,
                tenancyAttributes.ArrearsAgreementStartDate);
            InsertArrearsActions(tenancyAttributes.TenancyRef, tenancyAttributes.LastActionCode,
                tenancyAttributes.LastActionDate);
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
