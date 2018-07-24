using System;
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

        public List<ArrearsActionDiaryDetails> GetActionDiaryDetailsbyTenancyRefs(string tenancyRef)
        {
            var actionDiaryDetails = new List<ArrearsActionDiaryDetails>();

            try
            {
                actionDiaryDetails.Add(StoredActionDiaryDetails[tenancyRef]);
            }
            catch (Exception)
            {
                //do nothing
            }

            return actionDiaryDetails;
        }

        public List<PaymentTransactionDetails> GetPaymentTransactionsByTenancyRef(string tenancyRef)
        {
            var paymentTransactionDetails = new List<PaymentTransactionDetails>();

            try
            {
                paymentTransactionDetails.Add(StoredPaymentTransactionsDetails[tenancyRef]);
            }
            catch (Exception)
            {
                // do nothing
            }

            return paymentTransactionDetails;
        }

        public void SetTenancyListItem(string tenancyRef, TenancyListItem tenancyListItem)
        {
            StoredTenancyListItems[tenancyRef] = tenancyListItem;
        }

        public void SetPaymentTransactionDetails(string tenancyRef, PaymentTransactionDetails payment)
        {
            StoredPaymentTransactionsDetails[tenancyRef] = payment;
        }

        public void SetActionDiaryDetails(string tenancyRef,ArrearsActionDiaryDetails actionDiary)
        {
            StoredActionDiaryDetails[tenancyRef] = actionDiary;
        }

    }
}
