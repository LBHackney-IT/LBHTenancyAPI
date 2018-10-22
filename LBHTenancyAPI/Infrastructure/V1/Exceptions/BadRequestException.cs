using System.Net;
using LBHTenancyAPI.Infrastructure.Validation;

namespace LBHTenancyAPI.Infrastructure.Exceptions
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
