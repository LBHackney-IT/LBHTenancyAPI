using System.Collections.Generic;
using FluentValidation;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.Infrastructure.Validation;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class GetContactsForTenancyRequest:IRequest
    {
        public string TenancyAgreementReference { get; set; }

        public RequestValidationResponse Validate<T>(T request)
        {
            if(request == null)
                return new RequestValidationResponse(false, "request is null");
            var validator = new GetContactsForTenancyRequestValidator();
            var castedRequest =  request as GetContactsForTenancyRequest;
            var validationResult = validator.Validate(castedRequest);
            return new RequestValidationResponse(validationResult);
        }
    }

    public class GetContactsForTenancyRequestValidator : AbstractValidator<GetContactsForTenancyRequest>
    {
        public GetContactsForTenancyRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.TenancyAgreementReference).NotNull().NotEmpty();
        }
    }

    public class GetContactsForTenancyResponse
    {
        public IList<ContactsForTenancy> Contacts { get; set; }
    }

    public class ContactsForTenancy
    {
        public ContactsForTenancy(LBH.Data.Domain.Contact contact)
        {

        }
    }
}
