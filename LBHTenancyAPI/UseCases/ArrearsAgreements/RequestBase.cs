using System;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public abstract class RequestBase : IRequest
    {
        public virtual RequestValidationResponse Validate<T>(T request)
        {
            throw new NotImplementedException("This is the Generic Validate in the RequestBase");
        }
    }
}
