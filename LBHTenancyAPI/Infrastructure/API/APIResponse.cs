using System.Net;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Newtonsoft.Json;

namespace LBHTenancyAPI.Infrastructure.API
{
    public class APIResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("error")]
        public APIError Error { get; set; }

        public APIResponse(IExecuteWrapper<T> executeWrapper)
        {
            if (executeWrapper == null)
            {
                StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            if (executeWrapper?.Error != null)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError;
                Error = executeWrapper?.Error;
            }
        }
    }
}
