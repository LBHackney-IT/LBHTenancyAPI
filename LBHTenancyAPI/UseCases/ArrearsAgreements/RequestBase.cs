using System;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public abstract class RequestBase : IRequest
    {
        public virtual RequestValidationResponse IsValid()
        {
            throw new NotImplementedException("This is the Generic IsValid in the RequestBase");
        }
    }
}