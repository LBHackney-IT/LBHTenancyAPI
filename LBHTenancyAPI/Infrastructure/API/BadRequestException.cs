using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;

namespace LBHTenancyAPI.Infrastructure.API
{
    public class BadRequestException : ApiException
    {
        public IList<ExecutionError> Errors { get; set; }

        public BadRequestException()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequestException(IList<ValidationFailure> errors)
        {
            StatusCode = HttpStatusCode.BadRequest;
            Errors = new List<ExecutionError>();
            foreach (var validationFailure in errors)
            {
                var error = new ExecutionError(validationFailure);
                Errors.Add(error);
            }
        }
    }
}