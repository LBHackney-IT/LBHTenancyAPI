using LBHTenancyAPI.Settings.CRM;
using LBHTenancyAPI.Settings.Logging;
using LBHTenancyAPI.UseCases.Service;

namespace LBHTenancyAPI.Settings
{
    public class ConfigurationSettings
    {
        public Credentials.Credentials Credentials { get; set; }
        public ServiceSettings.ServiceSettings ServiceSettings { get; set; }
        public SentrySettings SentrySettings { get; set; }
        public Dynamics365Settings Dynamics365Settings { get; set; }
        public ServiceDetails ServiceDetailsSettings { get; set; }
    }
}
