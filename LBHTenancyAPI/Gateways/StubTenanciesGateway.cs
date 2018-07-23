using System.Collections.Generic;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public class StubTenanciesGateway : ITenanciesGateway
    {
        private Dictionary<string, TenancyListItem> StoredTenancyListItems;
        private Dictionary<string, ArrearsActionDiaryDetails> StoredActionDiaryDetails;
        private Dictionary<string, PaymentTransactionDetails> StoredPaymentTransactionsDetails;

        public StubTenanciesGateway()
        {
            StoredTenancyListItems = new Dictionary<string, TenancyListItem>();
            StoredActionDiaryDetails =  new Dictionary<string, ArrearsActionDiaryDetails>();
            StoredPaymentTransactionsDetails = new Dictionary<string, PaymentTransactionDetails>();
        }

        public List<TenancyListItem> GetTenanciesByRefs(List<string> tenancyRefs)
        {
            var tenancies = new List<TenancyListItem>();
            foreach (var tenancyRef in tenancyRefs)
            {
                tenancies.Add(StoredTenancyListItems[tenancyRef]);
            }

            return tenancies;
        }

        public List<ArrearsActionDiaryDetails> GetActionDiaryDetailsbyTenancyRefs(List<string> tenancyRefs)
        {
            var actionDiaryDetails = new List<ArrearsActionDiaryDetails>();
            foreach (var actionDiary in tenancyRefs)
            {
                actionDiaryDetails.Add(StoredActionDiaryDetails[actionDiary]);
            }

            return actionDiaryDetails;
        }

        public List<PaymentTransactionDetails> GetPaymentTransactionsByTenancyRef(List<string> tenancyRefs)
        {
            var paymentTransactionDetails = new List<PaymentTransactionDetails>();
            foreach (var paymentTrans in tenancyRefs)
            {
                paymentTransactionDetails.Add(StoredPaymentTransactionsDetails[paymentTrans]);
            }

            return paymentTransactionDetails;
        }

        public void SetTenancyListItem(string tenancyRef, TenancyListItem tenancyListItem)
        {
            StoredTenancyListItems[tenancyRef] = tenancyListItem;
        }
    }
}
