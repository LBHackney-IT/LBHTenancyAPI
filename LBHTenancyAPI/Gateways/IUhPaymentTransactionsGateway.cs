using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Gateways
{
    public interface IUhPaymentTransactionsGateway
    {
        string GetTransactionDescription(string transactionType);
    }
}
