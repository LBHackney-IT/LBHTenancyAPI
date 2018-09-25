using System;
namespace LBH.Data.Domain
{
    public class CRM365AccessTokenRequest
    {
        public string OrganizationUrl { get; set; }
        public string ClientId { get; set; }
        public string ApplicationKey { get; set; }
        public string ApplicationInstance { get; set; }
        public string TenantId { get; set; }
    }
}
