using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public interface ICreateArrearsAgreementUseCase:IUseCaseAsync<CreateArrearsAgreementRequest, CreateArrearsAgreementResponse>
    {

    }

    public class CreateArrearsAgreementUseCase : ICreateArrearsAgreementUseCase
    {
        public CreateArrearsAgreementUseCase()
        {

        }
        public async Task<IResponse<CreateArrearsAgreementResponse>> ExecuteAsync(CreateArrearsAgreementRequest request, CancellationToken cancellationToken)
        {
            //validate
            if(request == null || !request.IsValid())
                return new ExecuteResponse<CreateArrearsAgreementResponse>(request?.ValidationErrors);
            //execute business logic

            //
            return null;
        }
    }

    public class CreateArrearsAgreementRequest : RequestBase
    {
        public ArrearsAgreementInfo AgreementInfo { get; set; }
        public IList<ArrearsScheduledPaymentInfo> PaymentSchedule { get; set; }
        public override bool IsValid()
        {
            return false;
        }
    }

    public abstract class RequestBase : IRequest
    {
        public virtual bool IsValid()
        {
            throw new NotImplementedException("This is the Generic IsValid in the RequestBase");
        }
    }

    public class RequestValidation
    {
        public bool IsSuccess { get; set; }
        public IList<APIError> ValidationErrors { get; set; }
    }

    public class ExecuteResponse<T>: IResponse<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsValid { get; set; }
        public T Response { get; set; }
        public IList<APIError> Errors { get; set; }
        public IList<APIError> ValidationErrors { get; set; }

        public ExecuteResponse(T response)
        {
            Response = response;
            IsSuccess = true;
        }

        public ExecuteResponse(IList<APIError> errors)
        {
            Errors = errors;
            IsSuccess = false;
        }
    }

    public class CreateArrearsAgreementResponse
    {
        
    }

    public interface IUseCaseAsync<TRequest, TResponse> where TRequest: IRequest 
    {
        Task<IResponse<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequest
    {
        bool IsValid();
        IList<APIError> ValidationErrors { get; }
    }

    public interface IResponse<T>
    {
        bool IsSuccess { get; set; }
        bool IsValid { get; set; }
        T Response { get; set; }
        IList<APIError> Errors { get; set; }
        IList<APIError> ValidationErrors { get; set; }
    }
}
