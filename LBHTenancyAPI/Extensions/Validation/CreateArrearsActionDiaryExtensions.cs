using System;
using FluentValidation;
using AgreementService;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.Extensions.Validation
{
    /// <summary>
    /// Extension class for validating ArrearsActionCreateRequest requests
    /// </summary>
    [Obsolete("Please use the IRequest Pattern for Validating your requests")]
    public static class CreateArrearsActionDiaryExtensions
    {
        [Obsolete("Please use the IRequest Pattern for Validating your requests")]
        public static bool IsValid(this ArrearsActionCreateRequest request)
        {
            var validator = new ArrearsActionCreateRequestValidator();
            if (request?.ArrearsAction == null)
                return false;
            var result = validator.Validate(request);
            return result.IsValid;
        }

        [Obsolete("Please use the IRequest Pattern for Validating your requests")]
        public static bool IsValid(this ActionDiaryRequest request)
        {
            var validator = new ActionDiaryRequestValidator();
            if (request?.TenancyAgreementRef == null)
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

    public class ActionDiaryRequestValidator : AbstractValidator<ActionDiaryRequest>
    {
        public ActionDiaryRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.TenancyAgreementRef).NotEmpty().WithMessage("Please specify a tenancy reference");
            RuleFor(x => x.ActionCode).NotEmpty().NotNull().WithMessage("Please specify an action code");
            RuleFor(x => x.Comment).NotEmpty().NotNull().WithMessage("Please specify a comment");
        }
    }
}
