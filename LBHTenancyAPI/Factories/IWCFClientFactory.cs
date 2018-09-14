namespace LBHTenancyAPI.Factories
{
    public interface IWCFClientFactory
    {
        T CreateClient<T>(string endpoint);
    }
}
