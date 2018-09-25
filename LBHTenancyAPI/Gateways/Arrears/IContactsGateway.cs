using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Contact = LBH.Data.Domain.Contact;

namespace LBHTenancyAPI.Gateways.Arrears
{
    public interface IContactsGateway
    {
        Task<IList<Contact>> GetContactsByTenancyReferenceAsync(GetContactsForTenancyRequest request, CancellationToken cancellationToken);
    }

    public class ContactsGateway : IContactsGateway
    {
        public ContactsGateway()
        {

        }

        public Task<IList<Contact>> GetContactsByTenancyReferenceAsync(GetContactsForTenancyRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
