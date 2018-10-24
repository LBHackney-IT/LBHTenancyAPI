using FluentValidation;

namespace LBHTenancyAPI.UseCases.V1.Search.Models
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
