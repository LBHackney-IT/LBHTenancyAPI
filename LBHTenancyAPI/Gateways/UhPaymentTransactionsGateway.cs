using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LBHTenancyAPI.Gateways
{
    public class UhPaymentTransactionsGateway : IUhPaymentTransactionsGateway
    {
        public readonly ConcurrentDictionary<string, PaymentTransactionDescription> _transactions;

        public UhPaymentTransactionsGateway(string connectionString)
        {
            _transactions = new ConcurrentDictionary<string, PaymentTransactionDescription>();

            var transactionDescriptions = GetDescriptionsForTransactionCodesAsync(connectionString).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (var paymentTransactionDescription in transactionDescriptions)
            {
                _transactions.TryAdd(paymentTransactionDescription.Code, new PaymentTransactionDescription { Code = paymentTransactionDescription.Code, Description = paymentTransactionDescription.Description });
            }
        }

        public string GetTransactionDescription(string transactionType)
        {
            var unknownTransactionType = "Unknown transaction type";
            if (string.IsNullOrEmpty(transactionType))
                return unknownTransactionType;
            _transactions.TryGetValue(transactionType, out var transactionDescription);
            return transactionDescription != null ? transactionDescription.Description: unknownTransactionType;
        }

        public async Task<IList<PaymentTransactionDescription>> GetDescriptionsForTransactionCodesAsync(string connectionString)
        {
            var sqlConnection = new SqlConnection();
            sqlConnection.Open();

            var sb = new StringBuilder();
            sb.AppendLine("SELECT"); 
            sb.AppendLine("[deb_code] as Code,");
            sb.AppendLine("[deb_desc] as Description");
            sb.AppendLine("FROM[StubUH].[dbo].[debtype]");
            sb.AppendLine("UNION");
            sb.AppendLine("SELECT");
            sb.AppendLine("[rec_code] as Code,");
            sb.AppendLine("[rec_desc] as Description");
            sb.AppendLine("FROM[StubUH].[dbo].[rectype]");

            var sql = sb.ToString();
            var result = await sqlConnection.QueryAsync<PaymentTransactionDescription>(sql).ConfigureAwait(false);
            var paymentTransactionDescription = result.ToList();

            sqlConnection.Close();
            return paymentTransactionDescription;
        }
    }
}
