using FluentValidation;
using FluentValidation.Results;

namespace LBHTenancyAPI.Settings.CRM
{
    public class Dynamics365Settings
    {
        public string OrganizationUrl { get; set; }
        public string ClientId { get; set; }
        public string AppKey { get; set; }
        public string AadInstance { get; set; }
        public string TenantId { get; set; }
    }

    public static class Dynamic365SettingsExtensions
    {
        public static bool IsValid(this Dynamics365Settings instance)
        {
            if (instance == null)
                return false;
            var validator = new Dynamics365SettingsValidator();
            var validationResult = validator.Validate(instance);
            return validationResult.IsValid;
        }
    }

    public class Dynamics365SettingsValidator : AbstractValidator<Dynamics365Settings>
    {
        public Dynamics365SettingsValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.AadInstance).NotNull().NotEmpty().WithMessage("AadInstance cannot be null or empty");
            RuleFor(x => x.AppKey).NotNull().NotEmpty().WithMessage("AppKey cannot be null or empty");
            RuleFor(x => x.ClientId).NotNull().NotEmpty().WithMessage("ClientId cannot be null or empty");
            RuleFor(x => x.OrganizationUrl).NotNull().NotEmpty().WithMessage("OrganizationUrl cannot be null or empty");
            RuleFor(x => x.TenantId).NotNull().NotEmpty().WithMessage("TenantId cannot be null or empty");
        }
    }
}
