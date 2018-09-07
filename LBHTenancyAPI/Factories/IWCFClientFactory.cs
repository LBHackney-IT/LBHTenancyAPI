using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Factories
{
    public interface IWCFClientFactory
    {
        T CreateClient<T>(string endpoint);
    }

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
