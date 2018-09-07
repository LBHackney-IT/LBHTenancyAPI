using System.Collections.Generic;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class RequestValidationResponse
    {
        public bool IsValid { get; set; }
        public IList<APIError> ValidationErrors { get; set; }
    }
}