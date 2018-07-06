namespace LBHTenancyAPITest.Test.SqlGateway
{
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
    using LBHTenancyAPI.Models;
    using Xunit;

    public class SqlGatewayTest : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture db;

        public SqlGatewayTest(DatabaseFixture fixture)
        {
            db = fixture;
            db.Db.Open();
            db.Db.Query("Insert into Test values (1, 'banana')");
        }

        [Fact]
        public void GetAllTenanciesTest()
        {
            const string sql = "SELECT * FROM Test";
            List<Test> entries = db.Db.Query<Test>(sql).ToList();
                
            Assert.Single(entries);
            Assert.Equal(1, entries.First().Id);
            Assert.Equal("banana", entries.First().Name);            
        }
    }
}
