using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
using Xunit;
using Xunit.Abstractions;

namespace LBHTenancyAPITest.Test.Gateways
{
    public class UhTenanciesGatewayTest : IClassFixture<DatabaseFixture>
    {
        private readonly SqlConnection db;

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
            TenancyListItem expectedTenancy = CreateRandomTenancyListItem();
            InsertTenancyAttributes(expectedTenancy);

            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});

            Assert.Single(tenancies);

            Assert.Contains(expectedTenancy, tenancies);
        }

        [Fact]
        public void WhenGivenTenancyRef_GetTenanciesByRefs_ShouldReturnTheLatestAgreement()
        {
            TenancyListItem expectedTenancy = CreateRandomTenancyListItem();
            InsertTenancyAttributes(expectedTenancy);

            DateTime latestAragDate = expectedTenancy.ArrearsAgreementStartDate.AddDays(1);
            InsertAgreement(expectedTenancy.TenancyRef, "Inactive", expectedTenancy.ArrearsAgreementStartDate.Subtract(new TimeSpan(1, 0, 0, 0)));
            InsertAgreement(expectedTenancy.TenancyRef, "Active", latestAragDate);

            var tenancies = GetTenanciesByRef(new List<string> {expectedTenancy.TenancyRef});

            Assert.Equal(tenancies[0].ArrearsAgreementStartDate, latestAragDate);
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
            return new TenancyListItem()
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

        private void InsertTenancyAttributes(TenancyListItem tenancyAttributes)
        {
            string commandText =
                "INSERT INTO araction (tag_ref, action_code, action_date) VALUES (@tenancyRef, @lastActionType, @lastActionTime);" +
                "INSERT INTO tenagree (tag_ref, cur_bal) VALUES (@tenancyRef, @currentBalance);" +
                "INSERT INTO arag (tag_ref, arag_status, start_date) VALUES (@tenancyRef, @agreementStatus, @arrearsAgreementStartDate);" +
                "INSERT INTO contacts (tag_ref, con_name, con_address, con_postcode) VALUES (@tenancyRef, @primaryContactName, @primaryContactAddress, @primaryContactPostcode);";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.NVarChar);
            command.Parameters["@tenancyRef"].Value = tenancyAttributes.TenancyRef;
            command.Parameters.Add("@lastActionType", SqlDbType.NVarChar);
            command.Parameters["@lastActionType"].Value = tenancyAttributes.LastActionCode;
            command.Parameters.Add("@lastActionTime", SqlDbType.SmallDateTime);
            command.Parameters["@lastActionTime"].Value = tenancyAttributes.LastActionDate;
            command.Parameters.Add("@currentBalance", SqlDbType.NVarChar);
            command.Parameters["@currentBalance"].Value = tenancyAttributes.CurrentBalance;
            command.Parameters.Add("@agreementStatus", SqlDbType.NVarChar);
            command.Parameters["@agreementStatus"].Value = tenancyAttributes.ArrearsAgreementStatus;
            command.Parameters.Add("@arrearsAgreementStartDate", SqlDbType.SmallDateTime);
            command.Parameters["@arrearsAgreementStartDate"].Value = tenancyAttributes.ArrearsAgreementStartDate;
            command.Parameters.Add("@primaryContactName", SqlDbType.NVarChar);
            command.Parameters["@primaryContactName"].Value = tenancyAttributes.PrimaryContactName;
            command.Parameters.Add("@primaryContactAddress", SqlDbType.NVarChar);
            command.Parameters["@primaryContactAddress"].Value = tenancyAttributes.PrimaryContactShortAddress;
            command.Parameters.Add("@primaryContactPostcode", SqlDbType.NVarChar);
            command.Parameters["@primaryContactPostcode"].Value = tenancyAttributes.PrimaryContactPostcode;

            command.ExecuteNonQuery();
        }

        private void InsertAgreement(string tagRef, string status, DateTime startDate)
        {
            string commandText = "INSERT INTO arag (tag_ref, arag_status, start_date) VALUES (@tenancyRef, @agreementStatus, @startDate)";

            SqlCommand command = new SqlCommand(commandText, db);
            command.Parameters.Add("@tenancyRef", SqlDbType.NVarChar);
            command.Parameters["@tenancyRef"].Value = tagRef;
            command.Parameters.Add("@agreementStatus", SqlDbType.NVarChar);
            command.Parameters["@agreementStatus"].Value = status;
            command.Parameters.Add("@startDate", SqlDbType.SmallDateTime);
            command.Parameters["@startDate"].Value = startDate;

            command.ExecuteNonQuery();
        }
    }
}
