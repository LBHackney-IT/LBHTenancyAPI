using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases
{
    public interface IListTenancies
    {
        ListTenancies.Response Execute(List<string> tenancyRefs);
        ListTenancies.ArrearsActionDiaryResponse ExecuteActionDiaryQuery(List<string> TenancyRefs);
        ListTenancies.PaymentTransactionResponse ExecutePaymentTransactionQuery(List<string> TenancyRefs);
    }
}
