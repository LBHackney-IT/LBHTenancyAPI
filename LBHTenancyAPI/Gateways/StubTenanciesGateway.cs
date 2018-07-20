using System.Collections.Generic;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public class StubTenanciesGateway : ITenanciesGateway
    {
        private Dictionary<string, TenancyListItem> StoredTenancyListItems;
        private Dictionary<string, ArrearsActionDiaryDetails> StoredActionDiaryDetails;

        public StubTenanciesGateway()
        {
            StoredTenancyListItems = new Dictionary<string, TenancyListItem>();
            StoredActionDiaryDetails = new Dictionary<string, ArrearsActionDiaryDetails>();
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

        public List<ArrearsActionDiaryDetails> GetActionDiaryDetailsbyTenancyRef(List<string> tenancyRefs)
        {
            var actionDiaryDetails = new List<ArrearsActionDiaryDetails>();
            return actionDiaryDetails;
        }

        public void SetTenancyListItem(string tenancyRef, TenancyListItem tenancyListItem)
        {
            StoredTenancyListItems[tenancyRef] = tenancyListItem;
        }
    }
}
