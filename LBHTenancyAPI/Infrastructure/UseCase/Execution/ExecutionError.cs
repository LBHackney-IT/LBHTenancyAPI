using System;
using FluentValidation.Results;

namespace LBHTenancyAPI.Infrastructure.UseCase.Execution
{
    public class ExecutionError
    {
        public string Message { get; set; }
        public string Code { get; set; }

        public ExecutionError()
        {

        }

        public ExecutionError(ValidationFailure validationFailure)
        {
            Message = validationFailure?.ErrorMessage;
            
        }

        public ExecutionError(Exception ex)
        {
            Message = ex?.Message; 
        }

        public ExecutionError(AgreementService.WebResponse response)
        {
            Code = $"UH_{response?.ErrorCode}";
            Message = response?.ErrorMessage;
        }
    }
}
