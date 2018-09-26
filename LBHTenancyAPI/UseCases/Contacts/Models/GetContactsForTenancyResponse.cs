using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class GetContactsForTenancyResponse
    {
        public IList<ContactsForTenancy> Contacts { get; set; }
    }
}
