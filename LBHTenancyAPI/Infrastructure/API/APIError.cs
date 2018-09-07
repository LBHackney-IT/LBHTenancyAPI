using System;
using FluentValidation.Results;

namespace LBHTenancyAPI.Infrastructure.API
{
    public class APIError
    {
        public string Message { get; set; }
        public string Code { get; set; }

        public APIError()
        {

        }

        public APIError(ValidationFailure validationFailure)
        {
            Message = validationFailure?.ErrorMessage;
            
        }

        public APIError(Exception ex)
        {
            Message = ex?.Message; 
        }

        public APIError(AgreementService.WebResponse response)
        {
            Code = $"UH_{response?.ErrorCode}";
            Message = response?.ErrorMessage;
        }
    }
}