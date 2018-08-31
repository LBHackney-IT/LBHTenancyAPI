using AgreementService;
using FluentValidation;

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
            RuleFor(x => x.ArrearsAction).NotNull();
            RuleFor(x => x.ArrearsAction.TenancyAgreementRef).NotEmpty();
            RuleFor(x => x.ArrearsAction.ActionBalance).NotNull();
            RuleFor(x => x.ArrearsAction.ActionCode).NotEmpty().NotNull();
            RuleFor(x => x.ArrearsAction.Comment).NotEmpty().NotNull();
        }
    }
}
