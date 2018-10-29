namespace LBHTenancyAPI.Factories
{
    /// <summary>
    /// Factory for creating WCF clients
    /// </summary>
    public interface IWCFClientFactory
    {
        /// <summary>
        /// Creates a Client however doesn't open a connection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        T CreateClient<T>(string endpoint);
    }
}
