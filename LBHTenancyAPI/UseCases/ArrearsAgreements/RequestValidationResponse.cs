using System.Collections.Generic;
using FluentValidation.Results;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    /// <summary>
    /// Encapsulates a valid response using some sort of validation extensions
    /// We want this so we can swap out validation tools and still keep a standard response
    /// </summary>
    public class RequestValidationResponse
    {
        public bool IsValid { get; set; }
        public IList<ExecutionError> ValidationErrors { get; set; }

        public RequestValidationResponse(bool isValid)
        {
            IsValid = isValid;
        }
        public RequestValidationResponse(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ValidationErrors = new List<ExecutionError>();
            foreach (var validationResultError in validationResult.Errors)
            {
                var apiError = new ExecutionError(validationResultError);
                ValidationErrors.Add(apiError);
            }
        }
    }
}
