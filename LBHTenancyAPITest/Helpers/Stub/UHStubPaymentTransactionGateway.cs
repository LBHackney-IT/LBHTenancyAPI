using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;

namespace LBHTenancyAPITest.Helpers.Stub
{
    public class UHStubPaymentTransactionGateway : IRepository<PaymentTransaction>
    {
        private readonly SqlConnection _db;

        public UHStubPaymentTransactionGateway(SqlConnection db)
        {
            _db = db;
            if(db.State != ConnectionState.Open)
                db.Open();
        }

        public Task<PaymentTransaction> UpdateAsync(PaymentTransaction entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentTransaction> CreateAsync(PaymentTransaction entity, CancellationToken cancellationToken)
        {
            SqlCommand command = null;

            string commandText =
                "INSERT INTO rtrans (tag_ref, trans_ref, prop_ref, trans_type, post_date, real_value)" +
                "VALUES (@tenancyRef, @transRef, @propRef, @transType, @transactionDate, @amount)";

            command = new SqlCommand(commandText, _db);
            command.Parameters.Add("@tenancyRef", SqlDbType.Char);
            command.Parameters["@tenancyRef"].Value = entity.TenancyRef;

            command.Parameters.Add("@transRef", SqlDbType.Char);
            command.Parameters["@transRef"].Value = entity.TransactionRef;

            command.Parameters.Add("@transactionDate", SqlDbType.SmallDateTime);
            command.Parameters["@transactionDate"].Value = entity.Date;

            command.Parameters.Add("@propRef", SqlDbType.Char);
            command.Parameters["@propRef"].Value = entity.PropertyRef;

            command.Parameters.Add("@amount", SqlDbType.Decimal);
            command.Parameters["@amount"].Value = entity.Amount;

            command.Parameters.Add("@transType", SqlDbType.Char);
            command.Parameters["@transType"].Value = entity.Type;

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            return entity;

        }
    }

}
