using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHTenancyAPI.UseCases
{
    public interface IListAllPayments
    {
        Task<ListAllPayments.PaymentTransactionResponse> ExecuteAsync(string tenancyRef);
    }
}
