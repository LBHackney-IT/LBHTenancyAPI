using System.Collections.Generic;
using AgreementService;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class ExecuteWrapper<T>: IExecuteWrapper<T> where T:class
    {
        public T Response { get; set; }
        public IList<APIError> Errors { get; set; }
        public IList<ValidationError> ValidationErrors { get; set; }

        public ExecuteWrapper(T response)
        {
            
            if (typeof(T).IsSubclassOf(typeof(WebResponse)))
            {
                var webResponse =  response as WebResponse;
                if (!webResponse.Success)
                {
                    Errors = new List<APIError>(){new APIError()};
                }
                else
                {
                    Response = response;
                }
            }
            else
            {
                Response = response;
            }

        }

        public ExecuteWrapper(IList<APIError> errors)
        {
            Errors = errors;
        }

        public ExecuteWrapper(IList<ValidationError> validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}
