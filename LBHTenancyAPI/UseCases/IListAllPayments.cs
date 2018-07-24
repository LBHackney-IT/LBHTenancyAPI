using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases
{
    public interface IListAllPayments
    {
        AllPaymentsForTenancy.PaymentTransactionResponse Execute(string tenancyRef);
    }
}
