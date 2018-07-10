using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
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
            InsertAgreement(expectedTenancy.TenancyRef, "Inactive", expectedTenancy.ArrearsAgreementStartDate.Subtract(DAY_IN_TIMESPAN));
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

        private List<TenancyListItem> GetTenanciesByRef(List<string> refs)
        {
            var gateway = new UhTenanciesGateway(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            var tenancies = gateway.GetTenanciesByRefs(refs);

            return tenancies;
        }

        private TenancyListItem CreateRandomTenancyListItem()
        {
            var random = new Bogus.Randomizer();
            return new TenancyListItem
            {
                TenancyRef = random.Hash(),
                CurrentBalance = Math.Round(random.Double(), 2),
                LastActionDate = new DateTime(random.Int(1900, 1999), random.Int(1, 12), random.Int(1, 28), 9, 30, 0),
                LastActionCode = random.Hash(),
                ArrearsAgreementStatus = random.Word(),
                ArrearsAgreementStartDate = new DateTime(random.Int(1900, 1999), random.Int(1, 12), random.Int(1, 28), 9, 30, 0),
                PrimaryContactName = random.Word(),
                PrimaryContactShortAddress = random.Words(),
                PrimaryContactPostcode=  random.Word()

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
            command.Parameters.Add("@tenancyRef", SqlDbType.NVarChar);
            command.Parameters["@tenancyRef"].Value = tenancyAttributes.TenancyRef;
            command.Parameters.Add("@currentBalance", SqlDbType.NVarChar);
            command.Parameters["@currentBalance"].Value = tenancyAttributes.CurrentBalance;
            command.Parameters.Add("@primaryContactName", SqlDbType.NVarChar);
            command.Parameters["@primaryContactName"].Value = tenancyAttributes.PrimaryContactName;
            command.Parameters.Add("@primaryContactAddress", SqlDbType.NVarChar);
            command.Parameters["@primaryContactAddress"].Value = tenancyAttributes.PrimaryContactShortAddress;
            command.Parameters.Add("@primaryContactPostcode", SqlDbType.NVarChar);
            command.Parameters["@primaryContactPostcode"].Value = tenancyAttributes.PrimaryContactPostcode;

            command.ExecuteNonQuery();

            InsertAgreement(tenancyAttributes.TenancyRef, tenancyAttributes.ArrearsAgreementStatus, tenancyAttributes.ArrearsAgreementStartDate);
            InsertArrearsActions(tenancyAttributes.TenancyRef, tenancyAttributes.LastActionCode, tenancyAttributes.LastActionDate);
        }

        private void InsertAgreement(string tenancyRef, string status, DateTime startDate)
        {
            string commandText = "INSERT INTO arag (tag_ref, arag_status, start_date) VALUES (@tenancyRef, @agreementStatus, @startDate)";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.NVarChar);
            command.Parameters["@tenancyRef"].Value = tenancyRef;
            command.Parameters.Add("@agreementStatus", SqlDbType.NVarChar);
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
            command.Parameters.Add("@tenancyRef", SqlDbType.NVarChar);
            command.Parameters["@tenancyRef"].Value = tenancyRef;
            command.Parameters.Add("@actionCode", SqlDbType.NVarChar);
            command.Parameters["@actionCode"].Value = actionCode;
            command.Parameters.Add("@actionDate", SqlDbType.SmallDateTime);
            command.Parameters["@actionDate"].Value = actionDate;

            command.ExecuteNonQuery();
        }
    }
}
