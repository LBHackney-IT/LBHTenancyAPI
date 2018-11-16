using System.Net;
using LBHTenancyAPI.Infrastructure.V2.Validation;

namespace LBHTenancyAPI.Infrastructure.V2.Exceptions
{
    public class BadRequestException : APIException
    {
        public RequestValidationResponse ValidationResponse { get; set; }

        public BadRequestException():base(HttpStatusCode.BadRequest,"Request is null")
        {

        }

        public BadRequestException(RequestValidationResponse validationResponse)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ValidationResponse = validationResponse;
        }
    }
}
