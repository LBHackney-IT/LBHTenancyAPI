using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.Validation
{
    public static class APIErrorExtensions
    {
        public static IList<APIError> GetAPIErrors(this IList<ValidationFailure> validationFailures)
        {
            if (validationFailures == null)
                return null;
            var apiErrors = new List<APIError>(validationFailures.Count);
            foreach (var validationFailure in validationFailures)
            {
                var error = new APIError(validationFailure);
                apiErrors.Add(error);
            }

            return apiErrors;
        }
    }
}
