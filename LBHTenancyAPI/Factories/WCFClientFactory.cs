using System.ServiceModel;

namespace LBHTenancyAPI.Factories
{
    public class WCFClientFactory : IWCFClientFactory
    {
        public T CreateClient<T>(string endpoint)
        {
            var myBinding = new BasicHttpBinding();
            var myEndpoint = new EndpointAddress(endpoint);
            var myChannelFactory = new ChannelFactory<T>(myBinding, myEndpoint);
            T channel = myChannelFactory.CreateChannel();
            return channel;
        }
    }
}