using System.Collections.Generic;
using AgreementService;
using FluentValidation;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public class CreateArrearsAgreementRequest : RequestBase
    {
        public ArrearsAgreementInfo AgreementInfo { get; set; }
        public IList<ArrearsScheduledPaymentInfo> PaymentSchedule { get; set; }
        public override RequestValidationResponse Validate<T>(T request)
        {
            var validator = new CreateArrearsAgreementRequestValidator();
            var createRequest = request as CreateArrearsAgreementRequest;
            if (createRequest?.AgreementInfo == null || createRequest?.PaymentSchedule == null)
                return new RequestValidationResponse(false, "Agreement Info or Payment Schedule is null");
            var validationResult = validator.Validate(createRequest);
            return new RequestValidationResponse(validationResult);
        }

        public class CreateArrearsAgreementRequestValidator : AbstractValidator<CreateArrearsAgreementRequest>
        {
            public CreateArrearsAgreementRequestValidator()
            {
                RuleFor(x => x).NotNull();
                RuleFor(x => x.AgreementInfo).NotNull();
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
                RuleFor(x => x.PaymentSchedule).NotNull().WithMessage("Payment Schedule cannot be null");
                RuleFor(x => x.PaymentSchedule).NotEmpty().WithMessage("Payment Schedule cannot be empty");
                RuleForEach(x => x.PaymentSchedule).SetValidator(new ArrearsScheduledPaymentInfoValidator());
            }
        }

        public class ArrearsScheduledPaymentInfoValidator : AbstractValidator<ArrearsScheduledPaymentInfo>
        {
            public ArrearsScheduledPaymentInfoValidator()
            {
                RuleFor(x => x).NotNull();
                RuleFor(x => x.Amount).NotNull().WithMessage("Amount cannot be null");
                RuleFor(x => x.ArrearsFrequencyCode).NotNull().WithMessage("ArrearsFrequencyCode cannot be null");
                RuleFor(x => x.StartDate).NotNull().WithMessage("Start Date cannot be null");
                RuleFor(x => x.Comments).NotNull().WithMessage("Comments cannot be null");
            }
        }
    }
}
