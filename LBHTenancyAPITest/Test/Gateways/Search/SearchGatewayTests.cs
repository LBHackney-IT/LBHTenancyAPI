using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.Search;
using LBHTenancyAPI.UseCases.Contacts.Models;
using LBHTenancyAPITest.EF.Entities;
using LBHTenancyAPITest.Helpers;
using Xunit;
using ArrearsAgreement = LBH.Data.Domain.ArrearsAgreement;
using LBHTenancyAPITest.Helpers.Data;

namespace LBHTenancyAPITest.Test.Gateways.Search
{
    public class SearchGatewayTests : IClassFixture<DatabaseFixture>
    {
        readonly SqlConnection db;
        private static readonly TimeSpan DAY_IN_TIMESPAN = new TimeSpan(1, 0, 0, 0);
        private ISearchGateway _classUnderTest;

        public SearchGatewayTests(DatabaseFixture fixture)
        {
            db = fixture.Db;
            _classUnderTest = new SearchGateway(fixture.Db.ConnectionString);
        }

        [Theory]
        [InlineData("Smith")]
        [InlineData("Shetty")]
        public async Task can_search_on_last_name(string lastName)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            TestDataHelper.InsertMember(expectedMember, db);
            //arrears agreement
            var expectedArrearsAgreement = Fake.UniversalHousing.GenerateFakeArrearsAgreement();
            expectedArrearsAgreement.tag_ref = expectedTenancy.tag_ref;
            TestDataHelper.InsertAgreement(expectedArrearsAgreement, db);
            //arrears agreement det
            var expectedArrearsAgreementDet = Fake.UniversalHousing.GenerateFakeArrearsAgreementDet();
            expectedArrearsAgreementDet.tag_ref = expectedTenancy.tag_ref;
            TestDataHelper.InsertAgreementDet(expectedArrearsAgreementDet, db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 10,
                Page = 0
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNullOrEmpty();
            response[0].PrimaryContactName.Should().Contain(lastName);
        }

        [Theory]
        [InlineData("Jeff")]
        [InlineData("Rashmi")]
        public async Task can_search_on_first_name(string firstName)
        {
            //arrange
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();

            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();

            TestDataHelper.InsertMember(expectedMember, db);
            TestDataHelper.InsertProperty(expectedProperty, db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = "Jeff",
                PageSize = 10,
                Page = 0
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNullOrEmpty();
            response[0].PrimaryContactName.Should().Contain(firstName);
        }

        [Theory]
        [InlineData("E8 1HH")]
        [InlineData("E8 1EA")]
        public async Task can_search_on_post_code(string postCode)
        {
            //arrange
            //act
            //assert
        }
        [Theory]
        [InlineData("17 Reading Lane")]
        [InlineData("Hackney Contact Center")]
        public async Task can_search_on_short_address(string shortAddress)
        {
            //arrange
            //act
            //assert
        }
        [Theory]
        [InlineData("000017/01")]
        [InlineData("000018/01")]
        public async Task can_search_on_tenancy_reference(string shortAddress)
        {
            //arrange
            //act
            //assert
        }
        string InsertQueries()
        {
            string commandText =
                "INSERT INTO tenagree (tag_ref, prop_ref, cur_bal, tenure, rent, service, other_charge) VALUES (@tenancyRef, @propRef, @currentBalance, @tenure, @rent, @service, @otherCharge);" +
                "INSERT INTO contacts (tag_ref, con_name, con_phone1) VALUES (@tenancyRef, @primaryContactName, @primaryContactPhone);" +
                "INSERT INTO property (short_address, address1, prop_ref, post_code) VALUES (@primaryContactAddress, @primaryContactAddress, @propRef, @primaryContactPostcode);";
                
            return commandText;
        }
        
        private List<ArrearsAgreement> InsertRandomAgreementDetails(string tenancyRef, int num)
        {
            var random = new Faker();
            List<ArrearsAgreement> items = new List<ArrearsAgreement>();
            string commandText =
                "INSERT INTO arag (tag_ref, arag_status, arag_startdate, arag_startbal, arag_breached, arag_clearby) " +
                "VALUES (@tenancyRef, @agreementStatus, @startDate, @startBal, @breached, @clearBy)" +
                "INSERT INTO aragdet (arag_sid, aragdet_amount, aragdet_frequency) VALUES (@aragSid, @amount, @frequency)";
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
                command.Parameters.Add("@aragSid", SqlDbType.Int);
                command.Parameters["@aragSid"].Value = new Random().Next();
                items.Add(arrearsAgreement);
                command.ExecuteNonQuery();
            }
            return items.OrderByDescending(i => i.Startdate).ToList();
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
