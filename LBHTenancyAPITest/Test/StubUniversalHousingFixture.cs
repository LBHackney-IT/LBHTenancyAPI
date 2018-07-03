using System;
using System.Data.SqlClient;
using Dapper;

namespace LBHTenancyAPITest.Test
{
    public class DatabaseFixture : IDisposable
    {
        public SqlConnection Db { get; private set; }
        public DatabaseFixture()
        {
            SqlConnectionStringBuilder builder =  new SqlConnectionStringBuilder();  
            builder["Data Source"] = "(local)";
            builder["integrated Security"] = false;
            builder["Initial Catalog"] = "StubUH";
            builder.UserID = "sa";
            builder.Password = "Rooty-Tooty";
            Db = new SqlConnection(builder.ConnectionString);
        }

        /*
        * Called when test runner releases fixture
        */
        public void Dispose()
        {
            Db.Query("DELETE FROM Test");
            Db.Dispose();
        }
    }
}