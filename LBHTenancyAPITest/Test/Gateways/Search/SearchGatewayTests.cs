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
using LBHTenancyAPITest.EF;
using LBHTenancyAPITest.EF.Entities;
using LBHTenancyAPITest.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.Search
{
    public class SearchGatewayTests
    {
        private ISearchGateway _classUnderTest;
        private readonly UniversalHousingContext _universalHousingContext;

        public SearchGatewayTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder();
            var connectionString = "Server=JEFFPINKHAMD5D5\\SQLEXPRESS;Database=StubUH;Trusted_Connection=true";
            dbContextOptions.UseSqlServer(new SqlConnection(connectionString));
            _universalHousingContext = new UniversalHousingContext(dbContextOptions.Options);
            _classUnderTest = new SearchGateway(connectionString);
        }

        [Theory]
        [InlineData("Smith")]
        //[InlineData("Shetty")]
        public async Task can_search_on_last_name(string lastName)
        {
            //arrange
            var member = Fake.GenerateFakeMember();
            member.surname = lastName;
            var result = await _universalHousingContext.member.AddAsync(member, CancellationToken.None).ConfigureAwait(false);

            //var tenancyAgreement = new Faker<TenancyAgreement>();
            //var tenancyAgreementResult = await _universalHousingContext.tenagree.AddAsync(tenancyAgreement, CancellationToken.None).ConfigureAwait(false);

            var saveResult = await _universalHousingContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);
            //act
            var searchResponse = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName
            }, CancellationToken.None).ConfigureAwait(false);
            //assert
            searchResponse.Should().NotBeNullOrEmpty();
            searchResponse[0].PrimaryContactName.Should().Be(lastName);
        }

        [Theory]
        [InlineData("Jeff")]
        [InlineData("Rashmi")]
        public async Task can_search_on_first_name(string firstName)
        {
            //arrange

            //act

            //assert

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
    }
}
