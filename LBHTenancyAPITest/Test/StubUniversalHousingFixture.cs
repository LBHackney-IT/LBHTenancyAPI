using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Dapper;

namespace LBHTenancyAPITest.Test
{
    public class DatabaseFixture : IDisposable
    {
        public SqlConnection Db { get; private set; }

        public DatabaseFixture()
        {
            string dotenv = Path.GetRelativePath(Directory.GetCurrentDirectory(), "../../../../.env");
            DotNetEnv.Env.Load(dotenv);

            Db = new SqlConnection(DotNetEnv.Env.GetString("UH_CONNECTION_STRING"));
            Db.Open();
        }

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
