using System.Data.SqlClient;

namespace LBHTenancyAPI.Factories
{
    /// <summary>
    /// Factory for creating SQL connections 
    /// </summary>
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Creates an instance of a SQL connection object without opening a connection
        /// </summary>
        /// <returns></returns>
        SqlConnection Create();
    }
}
