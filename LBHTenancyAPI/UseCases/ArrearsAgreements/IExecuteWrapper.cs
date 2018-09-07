using System.Collections.Generic;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public interface IExecuteWrapper<T>
    {
        T Response { get; set; }
        IList<APIError> Errors { get; set; }
        IList<ValidationError> ValidationErrors { get; set; }
    }
}