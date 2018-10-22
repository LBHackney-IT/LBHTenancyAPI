using System.Net;
using System.Net.Http;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using LBHTenancyAPI.Infrastructure.V1.UseCase.Execution;

namespace LBHTenancyAPI.Infrastructure.V1.Dynamics365.Exceptions
{
    public class Dynamics365RestApiException : APIException
    {
        public ExecutionError ExecutionError { get; set; }

        public Dynamics365RestApiException(HttpResponseMessage response)
        {
            StatusCode = HttpStatusCode.BadGateway;
            ExecutionError = new ExecutionError(response);
        }
    }
}
