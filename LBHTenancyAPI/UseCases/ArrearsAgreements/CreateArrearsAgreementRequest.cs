using System.Collections.Generic;
using AgreementService;
using FluentValidation;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class CreateArrearsAgreementRequest : RequestBase
    {
        public ArrearsAgreementInfo AgreementInfo { get; set; }
        public IList<ArrearsScheduledPaymentInfo> PaymentSchedule { get; set; }
        public override RequestValidationResponse Validate()
        {
            var validator = new CreateArrearsAgreementRequestValidator();
            var validationResult = validator.Validate(this);
            return new RequestValidationResponse(validationResult);
        }

        public class CreateArrearsAgreementRequestValidator : AbstractValidator<CreateArrearsAgreementRequest>
        {
            public CreateArrearsAgreementRequestValidator()
            {
                RuleFor(x => x).NotNull();
                RuleFor(x => x.AgreementInfo).NotNull().WithMessage("AgreementInfo cannot be null");
                RuleFor(x => x.PaymentSchedule).NotEmpty().WithMessage("Please specify a payment schedule");
            }
        }
    }
}
