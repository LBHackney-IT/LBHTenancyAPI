using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.Search;
using LBHTenancyAPI.UseCases.Contacts.Models;
using LBHTenancyAPITest.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace LBHTenancyAPITest.Test.Gateways.Search
{
    public class SearchGatewayTests
    {
        private ISearchGateway _classUnderTest;
        private UniversalHousingContext _universalHousingContext;


        public SearchGatewayTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder();
            dbContextOptions.UseSqlServer(new SqlConnection("Server=JEFFPINKHAMD5D5\\SQLEXPRESS;Database=StubUH;Trusted_Connection=true"));
            _universalHousingContext = new UniversalHousingContext(dbContextOptions.Options);
        }

        [Theory]
        [InlineData("Smith")]
        [InlineData("Shetty")]
        public async Task can_search_on_last_name(string lastName)
        {
            //arrange
            var member = await _universalHousingContext.Members.AddAsync(new Member
            {
                
                bank_acc_type = "TES",
                house_ref = "10",
                person_no = 1

            }, CancellationToken.None).ConfigureAwait(false);
            //act

            //assert

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
