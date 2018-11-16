using System.Collections.Generic;
using FluentValidation.Results;

namespace LBHTenancyAPI.Infrastructure.V2.Validation
{
    /// <summary>
    /// Encapsulates a valid response using some sort of validation extensions
    /// We want this so we can swap out validation tools and still keep a standard response
    /// </summary>
    public class RequestValidationResponse
    {
        public bool IsValid { get; set; }
        public IList<ValidationError> ValidationErrors { get; set; }

        public RequestValidationResponse(bool isValid)
        {
            IsValid = isValid;
            ValidationErrors = new List<ValidationError>
            {
                new ValidationError
                {
                    Message = "request is null or required sub objects are null"
                }
            };
        }

        public RequestValidationResponse(bool isValid, string message)
        {
            IsValid = isValid;
            ValidationErrors = new List<ValidationError>
            {
                new ValidationError
                {
                    Message = message
                }
            };
        }
        public RequestValidationResponse(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ValidationErrors = new List<ValidationError>();
            foreach (var validationResultError in validationResult.Errors)
            {
                var apiError = new ValidationError(validationResultError);
                ValidationErrors.Add(apiError);
            }
        }
    }
}
