using System.Data.SqlClient;

namespace LBHTenancyAPI.Factories
{
    /// <summary>
    /// Factory for creating SQL connections 
    /// </summary>
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Creates a SQL connection however doesn't open it
        /// </summary>
        /// <returns></returns>
        SqlConnection Create();
    }
}
