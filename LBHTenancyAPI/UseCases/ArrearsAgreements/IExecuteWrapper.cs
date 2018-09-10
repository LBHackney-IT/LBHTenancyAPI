namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public interface IExecuteWrapper<T>
    {
        bool IsSuccess { get; set; }
        T Result { get; set; }
        APIError Error { get; set; }
    }
}
