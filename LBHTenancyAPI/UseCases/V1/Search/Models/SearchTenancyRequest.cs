using System.ComponentModel.DataAnnotations;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.Infrastructure.V1.Validation;

namespace LBHTenancyAPI.UseCases.V1.Search.Models
{
    /// <summary>
    /// SearchTenancyRequest V1 uses 1 parameter to search 5 fields
    /// One of the fields must be populated
    /// Self Validating
    /// </summary>
    public class SearchTenancyRequest : IRequest, IPagedRequest
    {
        /// <summary>
        /// Searches for tenants attached to tenancies
        /// Searches on 5 fields:
        /// FirstName - exact match OR
        /// LastName - exact match OR
        /// TenancyRef - exact match OR
        /// Postcode - partial match (contains) OR
        /// Address - partial match (contains) OR
        /// Orders by LastName, FirstName Desc
        /// Returns Individual Tenants attached to a tenancy so can return duplicate tenancies
        /// - Tenancy A - Tenant1
        /// - Tenancy A - Tenant2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Required]
        public string SearchTerm { get; set; }

        public RequestValidationResponse Validate<T>(T request)
        {
            if (request == null)
                return new RequestValidationResponse(false, "request is null");
            var validator = new SearchTenancyRequestValidator();
            var castedRequest = request as SearchTenancyRequest;
            var validationResult = validator.Validate(castedRequest);
            if (castedRequest.Page == 0)
                castedRequest.Page = 1;
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
