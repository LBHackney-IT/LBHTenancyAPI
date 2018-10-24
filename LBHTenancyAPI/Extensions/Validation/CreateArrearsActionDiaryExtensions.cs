using FluentValidation;
using AgreementService;

namespace LBHTenancyAPI.Extensions.Validation
{
    public static class CreateArrearsActionDiaryExtensions
    {
        public static bool IsValid(this ArrearsActionCreateRequest request)
        {
            var validator = new ArrearsActionCreateRequestValidator();
            if (request?.ArrearsAction == null)
                return false;
            var result = validator.Validate(request);
            return result.IsValid;
        }
    }

    public class ArrearsActionCreateRequestValidator : AbstractValidator<ArrearsActionCreateRequest>
    {
        public ArrearsActionCreateRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.ArrearsAction).NotNull().WithMessage("Arrears Action cannot be null");
            RuleFor(x => x.ArrearsAction.TenancyAgreementRef).NotEmpty().WithMessage("Please specify a tenancy reference");
            RuleFor(x => x.ArrearsAction.ActionCode).NotEmpty().NotNull().WithMessage("Please specify an action code");
            RuleFor(x => x.ArrearsAction.Comment).NotEmpty().NotNull().WithMessage("Please specify a comment");
        }
    }
}
