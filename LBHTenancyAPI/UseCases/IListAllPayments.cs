using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases
{
    public interface IListAllPayments
    {
        ListAllPayments.PaymentTransactionResponse Execute(string tenancyRef);
    }
}
