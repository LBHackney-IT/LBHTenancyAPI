namespace LBHTenancyAPITest.Test
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using Dapper;

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            string dotenv = Path.GetRelativePath(Directory.GetCurrentDirectory(), "../../../../.env");
            DotNetEnv.Env.Load(dotenv);

            Db = new SqlConnection(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            Db.Open();
        }

       public SqlConnection Db { get; }

        public void Dispose()
        {
            Db.Query(
                "DELETE FROM araction;" +
                "DELETE FROM arag;" +
                "DELETE FROM contacts;" +
                "DELETE FROM tenagree"
            );

            Db.Close();
            Db.Dispose();
        }
    }
}
