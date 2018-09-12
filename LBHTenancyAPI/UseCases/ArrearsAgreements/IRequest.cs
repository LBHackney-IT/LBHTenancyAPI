namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public interface IRequest
    {
        RequestValidationResponse Validate<T>(T request);
    }
}
