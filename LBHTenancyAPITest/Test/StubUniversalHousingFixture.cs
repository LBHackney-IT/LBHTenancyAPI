namespace LBHTenancyAPITest.Test
{
    using System;
    using System.Data.SqlClient;
    using Dapper;

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                ["Data Source"] = "(local)",
                ["integrated Security"] = false,
                ["Initial Catalog"] = "StubUH",
                UserID = "sa",
                Password = "Rooty-Tooty"
            };
            

            if (Environment.GetEnvironmentVariable("CI_TEST") == "True")
            {
                builder["Data Source"] = "tcp:stubuniversalhousing";
            }

            Db = new SqlConnection(builder.ConnectionString);
        }

        public SqlConnection Db { get; }

        public void Dispose()
        {
            Db.Query("DELETE FROM Test");

            Db.Dispose();
        }
    }
}
