using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;
using Contact = LBH.Data.Domain.Contact;

namespace LBHTenancyAPI.Gateways.V1.Contacts
{
    public interface IContactsGateway
    {
        Task<IList<Contact>> GetContactsByTenancyReferenceAsync(GetContactsForTenancyRequest request, CancellationToken cancellationToken);
    }
}
