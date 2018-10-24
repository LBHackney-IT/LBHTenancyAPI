using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.V1.Contacts.Models
{
    public class GetContactsForTenancyResponse
    {
        public IList<ContactsForTenancy> Contacts { get; set; }
    }
}
