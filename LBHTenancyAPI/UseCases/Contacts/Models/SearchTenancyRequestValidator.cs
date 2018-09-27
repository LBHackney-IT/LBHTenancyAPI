using FluentValidation;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class SearchTenancyRequestValidator : AbstractValidator<SearchTenancyRequest>
    {
        public SearchTenancyRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.SearchTerm).NotNull().NotEmpty();
        }
    }
}