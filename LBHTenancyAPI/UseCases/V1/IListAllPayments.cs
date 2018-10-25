using System.Threading.Tasks;

namespace LBHTenancyAPI.UseCases.V1
{
    public interface IListAllPayments
    {
        Task<ListAllPayments.PaymentTransactionResponse> ExecuteAsync(string tenancyRef);
    }
}
