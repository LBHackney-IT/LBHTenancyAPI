using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LBHTenancyAPI.Gateways.Search;
using LBHTenancyAPI.UseCases.Contacts.Models;
using LBHTenancyAPITest.Helpers;
using Xunit;
using LBHTenancyAPITest.Helpers.Data;

namespace LBHTenancyAPITest.Test.Gateways.Search
{
    public class SearchGatewayTests : IClassFixture<DatabaseFixture>
    {
        readonly SqlConnection db;
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
            expectedMember.forename = firstName;
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
                SearchTerm = firstName,
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
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            expectedProperty.post_code = postCode;
            TestDataHelper.InsertProperty(expectedProperty, db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            
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
                SearchTerm = postCode,
                PageSize = 10,
                Page = 0
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNullOrEmpty();
            response[0].PrimaryContactPostcode.Should().Contain(postCode);
        }

        [Theory]
        [InlineData("17 Reading Lane")]
        [InlineData("Hackney Contact Center")]
        public async Task can_search_on_short_address(string shortAddress)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            expectedProperty.short_address = shortAddress;
            TestDataHelper.InsertProperty(expectedProperty, db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
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
                SearchTerm = shortAddress,
                PageSize = 10,
                Page = 0
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNullOrEmpty();
            response[0].PrimaryContactShortAddress.Should().Contain(shortAddress);
        }

        [Theory]
        [InlineData("000017/01")]
        [InlineData("000018/01")]
        public async Task can_search_on_tenancy_reference(string tagRef)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.tag_ref = tagRef;
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
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
                SearchTerm = tagRef,
                PageSize = 10,
                Page = 0
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNullOrEmpty();
            response[0].TenancyRef.Should().Contain(tagRef);
        }
    }
}
