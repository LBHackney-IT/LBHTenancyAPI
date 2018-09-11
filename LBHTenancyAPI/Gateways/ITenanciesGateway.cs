using System.Collections.Generic;
using System.Threading.Tasks;
using LBH.Data.Domain;

namespace LBHTenancyAPI.Gateways
{
    public interface ITenanciesGateway
    {
        List<TenancyListItem> GetTenanciesByRefs(List<string> tenancyRefs);
        List<ArrearsActionDiaryEntry> GetActionDiaryEntriesbyTenancyRef(string tenancyRef);
        Task<List<PaymentTransaction>> GetPaymentTransactionsByTenancyRefAsync(string tenancyRef);
        Tenancy GetTenancyForRef(string tenancyRef);
    }
}
