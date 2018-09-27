using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.Infrastructure.Validation;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class SearchTenancyRequest : IRequest
    {
        public string SearchTerm { get; set; }

        public RequestValidationResponse Validate<T>(T request)
        {
            if (request == null)
                return new RequestValidationResponse(false, "request is null");
            var validator = new SearchTenancyRequestValidator();
            var castedRequest = request as SearchTenancyRequest;
            var validationResult = validator.Validate(castedRequest);
            return new RequestValidationResponse(validationResult);
        }
    }
}