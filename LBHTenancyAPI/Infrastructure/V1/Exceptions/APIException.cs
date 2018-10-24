using System;
using System.Net;

namespace LBHTenancyAPI.Infrastructure.V1.Exceptions
{
    public class APIException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public APIException()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public APIException(Exception ex):base(ex?.Message, ex)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public APIException(HttpStatusCode status)
        {
            StatusCode = status;
        }

        public APIException(HttpStatusCode statusCode, string message):base(message)
        {
            StatusCode = statusCode;
        }
    }
}
