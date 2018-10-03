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
            //member
            var member = Fake.UniversalHousing.GenerateFakeMember();
            member.surname = lastName;
            await _universalHousingContext.member.AddAsync(member, CancellationToken.None).ConfigureAwait(false);
            //property
            var property = Fake.UniversalHousing.GenerateFakeProperty();
            await _universalHousingContext.property.AddAsync(property, CancellationToken.None).ConfigureAwait(false);
            //tenancy
            var tenancyAgreement = Fake.UniversalHousing.GenerateFakeTenancy();
            tenancyAgreement.house_ref = member.house_ref;
            tenancyAgreement.prop_ref = property.prop_ref;
            await _universalHousingContext.tenagree.AddAsync(tenancyAgreement, CancellationToken.None).ConfigureAwait(false);
            //arrears agreement
            var arrearsAgreement = Fake.UniversalHousing.GenerateFakeArrearsAgreement();
            await _universalHousingContext.arag.AddAsync(arrearsAgreement, CancellationToken.None).ConfigureAwait(false);
            //save
            await _universalHousingContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);
            //act
            var searchResponse = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = lastName,
                Page = 0,
                PageSize = 10
            }, CancellationToken.None).ConfigureAwait(false);
            
            //assert
            searchResponse.Should().NotBeNullOrEmpty();
            searchResponse[0].PrimaryContactName.Should().Contain(lastName);
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
