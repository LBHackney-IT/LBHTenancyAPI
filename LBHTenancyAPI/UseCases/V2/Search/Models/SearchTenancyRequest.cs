using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.Infrastructure.V1.Validation;

namespace LBHTenancyAPI.UseCases.V2.Search.Models
{
    /// <summary>
    /// SearchTenancyRequest V2 uses 5 Fields to search on allowing for greater filtering
    /// One of the fields must be populated
    /// Validated by Validate Method
    /// </summary>
    public class SearchTenancyRequest : IRequest, IPagedRequest
    {
        /// <summary>
        /// Exact match
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Exact match
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Partial match (contains)
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Partial match (contains)
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Exact match
        /// </summary>
        public string TenancyRef { get; set; }

        /// <summary>
        /// Responsible for validating itself.
        /// Uses SearchTenancyRequestValidator to do complex validation
        /// Sets defaults for Page and PageSize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns>RequestValidationResponse</returns>
        public RequestValidationResponse Validate<T>(T request)
        {
            if (request == null)
                return new RequestValidationResponse(false, "request is null");
            var validator = new SearchTenancyRequestValidator();
            var castedRequest = request as SearchTenancyRequest;
            if(castedRequest == null)
                return new RequestValidationResponse(false);
            var validationResult = validator.Validate(castedRequest);
            //Using 1 based paging (to make it easier for Front Ends to page) so defaults to 1 instead of 0
            //Later down the stack we revert to 0 based paging
            if (castedRequest.Page == 0)
                castedRequest.Page = 1;
            //Sets default page size to 10
            if (castedRequest.PageSize == 0)
                castedRequest.PageSize = 10;
            return new RequestValidationResponse(validationResult);
        }
        /// <summary>
        /// Page defaults to 1 as paging is 1 index based not 0 index based
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// PageSize defaults to 10 
        /// </summary>
        public int PageSize { get; set; }
    }
}
