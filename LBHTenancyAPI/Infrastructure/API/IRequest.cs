using LBHTenancyAPI.Infrastructure.Validation;
using LBHTenancyAPI.UseCases.ArrearsAgreements;

namespace LBHTenancyAPI.Infrastructure.API
{
    /// <summary>
    /// Request Interface
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Encapsulates the validation by making the request responsible for validating itself
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        RequestValidationResponse Validate<T>(T request);
    }

    public interface IPagedRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }

    public interface IPagedResponse
    {
        int Page { get; set; }
        int PageCount { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
    }
}
