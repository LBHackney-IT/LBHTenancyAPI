using System.Net;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Authentication.Exceptions
{
    public class Dynamics365IsNotConfiguredException : APIException
    {
        public Dynamics365IsNotConfiguredException()
            : base(HttpStatusCode.InternalServerError,
                "Dynamics 365 Settings are not configured correctly")
        { }
    }
}
