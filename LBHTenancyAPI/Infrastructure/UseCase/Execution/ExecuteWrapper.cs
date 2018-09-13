using AgreementService;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;
using System;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class ExecuteWrapper<T>: IExecuteWrapper<T> where T:class
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public APIError Error { get; set; }

        public ExecuteWrapper(T response)
        {
            if (typeof(T).IsSubclassOf(typeof(WebResponse)))
            {
                var webResponse = response as WebResponse;
                if (!webResponse.Success)
                    Error = new APIError(webResponse);
                else
                {
                    IsSuccess = true;
                    Result = response;
                }
                    
            }
            else
            {
                IsSuccess = true;
                Result = response;
            }
                
        }

        public ExecuteWrapper(RequestValidationResponse validationResponse)
        {
            Error = new APIError(validationResponse);
        }

        public ExecuteWrapper(WebResponse response)
        {
            if (!response.Success)
                Error = new APIError(response);
        }

        public ExecuteWrapper(Exception ex)
        {
            Error = new APIError(ex);
        }

        public ExecuteWrapper(ExecutionError ex)
        {
            Error = new APIError(ex);
        }
    }
}
