using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.Infrastructure.Validation;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class SearchTenancyRequest : IRequest, IPagedRequest
    {
        public string SearchTerm { get; set; }

        public RequestValidationResponse Validate<T>(T request)
        {
            if (request == null)
                return new RequestValidationResponse(false, "request is null");
            var validator = new SearchTenancyRequestValidator();
            var castedRequest = request as SearchTenancyRequest;
            var validationResult = validator.Validate(castedRequest);
            if (PageSize == 0)
                PageSize = 10;
            return new RequestValidationResponse(validationResult);
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
