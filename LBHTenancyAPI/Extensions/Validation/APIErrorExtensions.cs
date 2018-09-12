using System.Collections.Generic;
using FluentValidation.Results;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;

namespace LBHTenancyAPI.Extensions.Validation
{
    public static class APIErrorExtensions
    {
        public static IList<ExecutionError> GetAPIErrors(this IList<ValidationFailure> validationFailures)
        {
            if (validationFailures == null)
                return null;
            var apiErrors = new List<ExecutionError>(validationFailures.Count);
            foreach (var validationFailure in validationFailures)
            {
                var error = new ExecutionError(validationFailure);
                apiErrors.Add(error);
            }

            return apiErrors;
        }
    }
}
