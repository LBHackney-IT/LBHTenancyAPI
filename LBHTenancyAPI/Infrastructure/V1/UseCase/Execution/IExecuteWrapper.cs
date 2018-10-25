using LBHTenancyAPI.Infrastructure.V1.API;

namespace LBHTenancyAPI.Infrastructure.V1.UseCase.Execution
{
    public interface IExecuteWrapper<T>
    {
        bool IsSuccess { get; set; }
        T Result { get; set; }
        APIError Error { get; set; }
    }
}
