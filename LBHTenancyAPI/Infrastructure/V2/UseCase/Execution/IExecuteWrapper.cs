using LBHTenancyAPI.Infrastructure.V2.API;

namespace LBHTenancyAPI.Infrastructure.V2.UseCase.Execution
{
    public interface IExecuteWrapper<T>
    {
        bool IsSuccess { get; set; }
        T Result { get; set; }
        APIError Error { get; set; }
    }
}
