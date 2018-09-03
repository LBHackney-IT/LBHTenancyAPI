using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public interface ITenanciesGateway
    {
        Task<List<TenancyListItem>> GetTenanciesByRefsAsync(List<string> tenancyRefs);
        List<ArrearsActionDiaryEntry> GetActionDiaryEntriesbyTenancyRef(string tenancyRef);
        List<PaymentTransaction> GetPaymentTransactionsByTenancyRef(string tenancyRef);
        Tenancy GetTenancyForRef(string tenancyRef);
    }
}
