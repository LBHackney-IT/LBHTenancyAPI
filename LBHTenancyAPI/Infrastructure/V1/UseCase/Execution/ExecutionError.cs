using System;
using System.Net.Http;
using AgreementService;
using FluentValidation.Results;

namespace LBHTenancyAPI.Infrastructure.V1.UseCase.Execution
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

        public ExecutionError(WebResponse response)
        {
            Code = $"UH_{response?.ErrorCode}";
            Message = response?.ErrorMessage;
        }

        public ExecutionError(HttpResponseMessage response)
        {
            var message = string.Empty;
            if (response?.Content != null)
                message = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Code = $"Dynamics365_HttpStatus_{response?.StatusCode}";
            Message = message;
        }
    }
}
