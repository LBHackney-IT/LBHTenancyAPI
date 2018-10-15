using System.Data.SqlClient;

namespace LBHTenancyAPI.Factories
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}