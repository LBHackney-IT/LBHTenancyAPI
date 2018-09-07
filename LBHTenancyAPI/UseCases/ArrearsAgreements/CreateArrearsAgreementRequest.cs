using System.Collections.Generic;
using AgreementService;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class CreateArrearsAgreementRequest : RequestBase
    {
        public ArrearsAgreementInfo AgreementInfo { get; set; }
        public IList<ArrearsScheduledPaymentInfo> PaymentSchedule { get; set; }
        public override RequestValidationResponse IsValid()
        {
            return new RequestValidationResponse();
        }
    }
}
