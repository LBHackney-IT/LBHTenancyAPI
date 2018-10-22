using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways.Contacts;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;

namespace LBHTenancyAPI.UseCases.V1.Contacts
{
    public class GetContactsForTenancyUseCase: IGetContactsForTenancyUseCase
    {
        private readonly IContactsGateway _contactsGateway;

        public GetContactsForTenancyUseCase(IContactsGateway contactsGateway)
        {
            _contactsGateway = contactsGateway;
        }

        public async Task<GetContactsForTenancyResponse> ExecuteAsync(GetContactsForTenancyRequest request, CancellationToken cancellationToken)
        {
            //validate
            if (request == null)
                throw new BadRequestException();

            var validationResponse = request.Validate(request);
            if (!validationResponse.IsValid)
                throw new BadRequestException(validationResponse);

            var response = await _contactsGateway.GetContactsByTenancyReferenceAsync(request, cancellationToken).ConfigureAwait(false);

            //tenancy could have no attached contacts
            if(response == null)
                return new GetContactsForTenancyResponse();

            //Create real response
            var useCaseResponse = new GetContactsForTenancyResponse
            {
                Contacts = response.Select(s=> new ContactsForTenancy(s)).ToList()
            };

            return useCaseResponse;
        }
    }
}
