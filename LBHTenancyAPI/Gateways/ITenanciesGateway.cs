using System.Collections.Generic;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public interface ITenanciesGateway
    {
        List<TenancyListItem> GetTenanciesByRefs(List<string> tenancyRefs);
        List<ArrearsActionDiaryDetails> GetActionDiaryDetailsbyTenancyRefs(List<string> tenancyRefs);
    }
}
