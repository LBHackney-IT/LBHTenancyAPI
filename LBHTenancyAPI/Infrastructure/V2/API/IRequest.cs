using LBHTenancyAPI.Infrastructure.V2.Validation;

namespace LBHTenancyAPI.Infrastructure.V2.API
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
}
