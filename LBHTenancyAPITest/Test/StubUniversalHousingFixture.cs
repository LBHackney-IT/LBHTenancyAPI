using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true,MaxParallelThreads = 1)]

namespace LBHTenancyAPITest.Test
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using Dapper;


    public class DatabaseFixture : IDisposable
    {
        public SqlConnection Db { get; }
        public string ConnectionString { get; }

        public DatabaseFixture()
        {
            try
            {
                string dotenv = Path.GetRelativePath(Directory.GetCurrentDirectory(), "../../../../.env");
                DotNetEnv.Env.Load(dotenv);
            }
            catch (Exception)
            {
                Console.Write("Failed to load .env file for test");
            }

            ConnectionString = DotNetEnv.Env.GetString("UH_CONNECTION_STRING");
            Db = new SqlConnection(ConnectionString);

            Db.Open();
        }

        public void Dispose()
        {
            Db.Query(
                "DELETE FROM araction;" +
                "DELETE FROM arag;" +
                "DELETE FROM contacts;" +
                "DELETE FROM tenagree;" +
                "DELETE FROM aragdet;" +
                "DELETE FROM rtrans;" +
                "DELETE FROM member;" +
                "DELETE FROM property;"
            );

            Db.Close();
            Db.Dispose();
        }
    }
}
