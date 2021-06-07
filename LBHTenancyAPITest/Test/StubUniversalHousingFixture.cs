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
                Console.WriteLine("Failed to load .env file for test");
            }

            //ConnectionString = DotNetEnv.Env.GetString("UH_CONNECTION_STRING");
            ConnectionString = "Data Source=127.0.0.1;Initial Catalog=StubUH;User ID=housingadmin;Password=Vcf:8efGbuEv2qmD";
            Db = new SqlConnection(ConnectionString);

            Db.Open();
        }

        public void Dispose()
        {
            Db.Query(
                "DELETE FROM UHAraction;" +
                "DELETE FROM UHArag;" +
                "DELETE FROM UHContacts;" +
                "DELETE FROM UHTenancyAgreement;" +
                "DELETE FROM UHAragdet;" +
                "DELETE FROM UHMiniTransaction;" +
                "DELETE FROM UHMember;" +
                "DELETE FROM UHProperty;"
            );

            Db.Close();
            Db.Dispose();
        }
    }
}
