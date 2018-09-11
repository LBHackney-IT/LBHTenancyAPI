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
                RuleFor(x => x.AgreementInfo.ArrearsAgreementStatusCode).NotNull().WithMessage("Arrears Agreement Status Code cannot be null");
                RuleFor(x => x.AgreementInfo.StartBalance).NotNull().WithMessage("StartBalance cannot be null");
                RuleFor(x => x.AgreementInfo.StartDate).NotNull().WithMessage("Start Date cannot be null");
                RuleFor(x => x.AgreementInfo.Comment).NotEmpty().WithMessage("Comment cannot be empty");
                RuleFor(x => x.AgreementInfo.TenancyAgreementRef).NotEmpty().WithMessage("TenancyAgreementRef cannot be empty");
                RuleFor(x => x.AgreementInfo.IsBreached).NotEmpty().WithMessage("IsBreached cannot be empty");
                RuleFor(x => x.AgreementInfo.FirstCheck).NotEmpty().WithMessage("FirstCheck cannot be empty");
                RuleFor(x => x.AgreementInfo.FirstCheckFrequencyTypeCode).NotNull().WithMessage("FirstCheckFrequencyTypeCode cannot be null");
                RuleFor(x => x.AgreementInfo.NextCheck).NotNull().WithMessage("NextCheck cannot be null");
                RuleFor(x => x.AgreementInfo.FcaDate).NotNull().WithMessage("FCADate cannot be null");
                RuleFor(x => x.AgreementInfo.FcaDate).NotEmpty().WithMessage("FCADate is required")
                                       .GreaterThan(x => x.AgreementInfo.StartDate).WithMessage("FCA date must be after Start date")
                                       .When(x => x.AgreementInfo.StartDate.HasValue);
                RuleFor(x => x.AgreementInfo.MonitorBalanceCode).NotNull().WithMessage("MonitorBalanceCode cannot be null");

                RuleFor(x => x.PaymentSchedule).NotEmpty().WithMessage("Please specify a payment schedule");
                RuleFor(x => x.PaymentSchedule).NotNull().WithMessage("FCADate cannot be null");
            }
        }
    }
}
