using LBHTenancyAPI.Settings.CRM;
using LBHTenancyAPI.Settings.Logging;
using LBHTenancyAPI.UseCases.V1.Service;

namespace LBHTenancyAPI.Settings
{
    /// <summary>
    /// Represents a POCO object of the values set as configuration in the appsettings.json
    /// Allowing for configuration values to be easily used and mocked
    /// </summary>
    public class ConfigurationSettings
    {
        /// <summary>
        /// UH Service credentials
        /// </summary>
        public Credentials.Credentials Credentials { get; set; }
        /// <summary>
        /// UH Agreement Service 
        /// </summary>
        public ServiceSettings.ServiceSettings ServiceSettings { get; set; }
        /// <summary>
        /// Sentry error handling 
        /// </summary>
        public SentrySettings SentrySettings { get; set; }
        /// <summary>
        /// Dynamics 365 Settings
        /// </summary>
        public Dynamics365Settings Dynamics365Settings { get; set; }
        public ServiceDetails ServiceDetailsSettings { get; set; }
    }
}
