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
            try
            {
                string dotenv = Path.GetRelativePath(Directory.GetCurrentDirectory(), "../../../../.env");
                DotNetEnv.Env.Load(dotenv);
            }
            catch (Exception)
            {
                // do nothing
            }

            Db = new SqlConnection("Server=JEFFPINKHAMD5D5\\SQLEXPRESS;Database=StubUH;Trusted_Connection=True");

            Db.Open();
        }

       public SqlConnection Db { get; }

        public void Dispose()
        {
            Db.Query(
                "DELETE FROM araction;" +
                "DELETE FROM arag;" +
                "DELETE FROM contacts;" +
                "DELETE FROM tenagree;" +
                "DELETE FROM aragdet;" +
                "DELETE FROM rtrans;" +
                "DELETE FROM member;"
            );

            Db.Close();
            Db.Dispose();
        }
    }
}
