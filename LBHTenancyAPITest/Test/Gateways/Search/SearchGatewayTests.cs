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
        readonly SqlConnection _db;
        private readonly ISearchGateway _classUnderTest;

        public SearchGatewayTests(DatabaseFixture fixture)
        {
            _db = fixture.Db;
            var connection = DotNetEnv.Env.GetString("UH_CONNECTION_STRING");
            _classUnderTest = new SearchGateway(connection);
        }

        [Theory]
        [InlineData("000089/04", "000089/05")]
        [InlineData("000090/04", "000089/05")]
        public async Task search_returns_null_when_null_is_passed_in(string tenancyRef, string tenancyRef2)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            expectedTenancy.tag_ref = tenancyRef;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //tenancy
            var expectedTenancy2 = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy2.house_ref = expectedTenancy.house_ref;
            expectedTenancy2.prop_ref = expectedProperty.prop_ref;
            expectedTenancy2.tag_ref = tenancyRef2;
            TestDataHelper.InsertTenancy(expectedTenancy2, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertMember(expectedMember, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = null,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().BeNullOrEmpty();
        }

        [Theory]
        [InlineData("000089/02", "000089/03")]
        [InlineData("000090/02", "000089/03")]
        public async Task can_search_on_tenancy_ref(string tenancyRef, string tenancyRef2)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            expectedTenancy.tag_ref = tenancyRef;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //tenancy
            var expectedTenancy2 = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy2.house_ref = expectedTenancy.house_ref;
            expectedTenancy2.prop_ref = expectedProperty.prop_ref;
            expectedTenancy2.tag_ref = tenancyRef2;
            TestDataHelper.InsertTenancy(expectedTenancy2, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertMember(expectedMember, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = tenancyRef,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results.Count.Should().Be(1);
            response.Results[0].TenancyRef.Should().BeEquivalentTo(tenancyRef);
        }

        [Theory]
        [InlineData("Smith")]
        [InlineData("Shetty")]
        public async Task can_search_on_last_name(string lastName)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            TestDataHelper.InsertMember(expectedMember, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results[0].PrimaryContactName.Should().Contain(lastName);
        }

        [Theory]
        [InlineData("Jeff")]
        [InlineData("Rashmi")]
        public async Task can_search_on_first_name(string firstName)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.forename = firstName;
            TestDataHelper.InsertMember(expectedMember, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = firstName,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results[0].PrimaryContactName.Should().Contain(firstName);
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
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            
            TestDataHelper.InsertMember(expectedMember, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = postCode,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results[0].PrimaryContactPostcode.Should().Contain(postCode);
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
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertMember(expectedMember, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = shortAddress,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results[0].PrimaryContactShortAddress.Should().Contain(shortAddress);
        }

        [Theory]
        [InlineData("000017/01")]
        [InlineData("000018/01")]
        public async Task can_search_on_tenancy_reference(string tagRef)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.tag_ref = tagRef;
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertMember(expectedMember, _db);
            //arrears agreement
            var expectedArrearsAgreement = Fake.UniversalHousing.GenerateFakeArrearsAgreement();
            expectedArrearsAgreement.tag_ref = expectedTenancy.tag_ref;
            TestDataHelper.InsertAgreement(expectedArrearsAgreement, _db);
            //arrears agreement det
            var expectedArrearsAgreementDet = Fake.UniversalHousing.GenerateFakeArrearsAgreementDet();
            expectedArrearsAgreementDet.tag_ref = expectedTenancy.tag_ref;
            TestDataHelper.InsertAgreementDet(expectedArrearsAgreementDet, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = tagRef,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results[0].TenancyRef.Should().Contain(tagRef);
        }

        [Theory]
        [InlineData("Smith")]
        [InlineData("Shetty")]
        public async Task can_get_total_count(string lastName)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member 1
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            TestDataHelper.InsertMember(expectedMember, _db);
            //member 2
            var expectedMember2 = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember2.house_ref = expectedTenancy.house_ref;
            expectedMember2.surname = lastName;
            TestDataHelper.InsertMember(expectedMember2, _db);

            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 1,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.TotalResultsCount.Should().Be(2);
        }

        [Theory]
        [InlineData("Brady")]
        [InlineData("Donaldson")]
        public async Task can_search_even_with_no_arrears_agreement(string lastName)
        {
            //arrange
            //property
            var expectedProperty = Fake.UniversalHousing.GenerateFakeProperty();
            TestDataHelper.InsertProperty(expectedProperty, _db);
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            expectedTenancy.prop_ref = expectedProperty.prop_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member 1
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            TestDataHelper.InsertMember(expectedMember, _db);
            //member 2
            var expectedMember2 = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember2.house_ref = expectedTenancy.house_ref;
            expectedMember2.surname = lastName;
            TestDataHelper.InsertMember(expectedMember2, _db);
            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 1,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.TotalResultsCount.Should().Be(2);
        }

        [Theory]
        [InlineData("Glover")]
        [InlineData("Roark")]
        public async Task members_with_no_tenancy_agreements_are_not_returned(string lastName)
        {
            //arrange
            //member 1
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            
            expectedMember.surname = lastName;
            TestDataHelper.InsertMember(expectedMember, _db);
            //member 2
            var expectedMember2 = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember2.surname = lastName;
            TestDataHelper.InsertMember(expectedMember2, _db);
            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 1,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().BeNullOrEmpty();
        }

        /// <summary>
        /// This scenario came up when testing against staging database
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("Piper")]
        [InlineData("Tiper")]
        public async Task can_search_even_with_no_property_assigned(string lastName)
        {
            //arrange
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member 1
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            TestDataHelper.InsertMember(expectedMember, _db);
            //member 2
            var expectedMember2 = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember2.house_ref = expectedTenancy.house_ref;
            expectedMember2.surname = lastName;
            TestDataHelper.InsertMember(expectedMember2, _db);
            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 1,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.TotalResultsCount.Should().Be(2);
        }

        [Theory]
        [InlineData("Schrodinger", "Ally", "Bob")]
        [InlineData("Cat", "Alison", "Bon")]
        public async Task search_is_ordered_by_lastname_surname(string lastName, string firstName, string firstName2)
        {
            //arrange
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member 1
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            expectedMember.forename = firstName;
            TestDataHelper.InsertMember(expectedMember, _db);
            //member 2
            var expectedMember2 = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember2.house_ref = expectedTenancy.house_ref;
            expectedMember2.surname = lastName;
            expectedMember2.forename = firstName2;
            TestDataHelper.InsertMember(expectedMember2, _db);
            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.TotalResultsCount.Should().Be(2);
            response.Results[0].PrimaryContactName.Should().Contain(firstName);
            response.Results[1].PrimaryContactName.Should().Contain(firstName2);
        }

        [Theory]
        [InlineData("Rick", "Alternate", "Pickle")]
        [InlineData("Morty", "Funny", "Robot")]
        public async Task search_can_page_results(string lastName, string firstName, string firstName2)
        {
            //arrange
            //tenancy
            var expectedTenancy = Fake.UniversalHousing.GenerateFakeTenancy();
            expectedTenancy.house_ref = expectedTenancy.house_ref;
            TestDataHelper.InsertTenancy(expectedTenancy, _db);
            //member 1
            var expectedMember = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember.house_ref = expectedTenancy.house_ref;
            expectedMember.surname = lastName;
            expectedMember.forename = firstName;
            TestDataHelper.InsertMember(expectedMember, _db);
            //member 2
            var expectedMember2 = Fake.UniversalHousing.GenerateFakeMember();
            expectedMember2.house_ref = expectedTenancy.house_ref;
            expectedMember2.surname = lastName;
            expectedMember2.forename = firstName2;
            TestDataHelper.InsertMember(expectedMember2, _db);
            //act
            var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 1,
                Page = 1
            }, CancellationToken.None);

            var response2 = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                PageSize = 1,
                Page = 2
            }, CancellationToken.None);

            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.TotalResultsCount.Should().Be(2);
            response.Results[0].PrimaryContactName.Should().BeEquivalentTo($"{firstName} {lastName}");

            //assert second response
            response2.Should().NotBeNull();
            response2.Results.Should().NotBeNullOrEmpty();
            response2.TotalResultsCount.Should().Be(2);
            response2.Results[0].PrimaryContactName.Should().BeEquivalentTo($"{firstName2} {lastName}");
        }
    }
}
