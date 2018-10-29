using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.Service
{
    public class ServiceDetailVersion
    {
        public string Version { get; set; }
        public string GitCommitHash { get; set; }
        public IList<string> ApiVersions { get; set; }
    }
}
