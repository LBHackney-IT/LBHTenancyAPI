using FluentValidation;

namespace LBHTenancyAPI.UseCases.V2.Search.Models
{
    public class SearchTenancyRequestValidator : AbstractValidator<SearchTenancyRequest>
    {
        public SearchTenancyRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.TenancyRef).NotNull().NotEmpty().When(m => string.IsNullOrEmpty(m.FirstName) && string.IsNullOrEmpty(m.LastName) && string.IsNullOrEmpty(m.Address) && string.IsNullOrEmpty(m.PostCode)).WithMessage("Please enter a search term into TenancyRef");
            RuleFor(x => x.FirstName).NotNull().NotEmpty().When(m => string.IsNullOrEmpty(m.TenancyRef) && string.IsNullOrEmpty(m.LastName) && string.IsNullOrEmpty(m.Address) && string.IsNullOrEmpty(m.PostCode)).WithMessage("Please enter a search term into FirstName");
            RuleFor(x => x.LastName).NotNull().NotEmpty().When(m => string.IsNullOrEmpty(m.TenancyRef) && string.IsNullOrEmpty(m.FirstName) && string.IsNullOrEmpty(m.Address) && string.IsNullOrEmpty(m.PostCode)).WithMessage("Please enter a search term into LastName");
            RuleFor(x => x.Address).NotNull().NotEmpty().When(m => string.IsNullOrEmpty(m.TenancyRef) && string.IsNullOrEmpty(m.FirstName) && string.IsNullOrEmpty(m.LastName) && string.IsNullOrEmpty(m.PostCode)).WithMessage("Please enter a search term into Address");
            RuleFor(x => x.PostCode).NotNull().NotEmpty().When(m => string.IsNullOrEmpty(m.TenancyRef) && string.IsNullOrEmpty(m.FirstName) && string.IsNullOrEmpty(m.LastName) && string.IsNullOrEmpty(m.Address)).WithMessage("Please enter a search term into PostCode");
        }
    }
}
