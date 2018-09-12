using LBHTenancyAPI.Settings.Logging;

namespace LBHTenancyAPI.Settings
{
    public class ConfigurationSettings
    {
        public Credentials.Credentials Credentials { get; set; }
        public ServiceSettings.ServiceSettings ServiceSettings { get; set; }
        public SentrySettings SentrySettings { get; set; }
    }
}
