using System.Collections.Generic;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPITest.TestDoubles.Gateways
{
    public class StubTenanciesGateway : ITenanciesGateway
    {
        private Dictionary<string, TenancyListItem> StoredTenancyListItems;

        public StubTenanciesGateway()
        {
            StoredTenancyListItems = new Dictionary<string, TenancyListItem>();
        }

        public List<TenancyListItem> GetTenanciesByRefs(List<string> tenancyRefs)
        {
            var tenancies = new List<TenancyListItem>();
            foreach (var tenancyRef in tenancyRefs)
            {
                tenancies.Add(StoredTenancyListItems[tenancyRef]);
            }

            return tenancies;
        }

        public void SetTenancyListItem(string tenancyRef, TenancyListItem tenancyListItem)
        {
            StoredTenancyListItems[tenancyRef] = tenancyListItem;
        }
    }
}
