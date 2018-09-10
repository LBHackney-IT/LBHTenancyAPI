using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public class StubTenanciesGateway : ITenanciesGateway
    {
        private Dictionary<string, TenancyListItem> StoredTenancyListItems;
        private Dictionary<string, ArrearsActionDiaryEntry> StoredActionDiaryDetails;
        private Dictionary<string, PaymentTransaction> StoredPaymentTransactionsDetails;
        private Dictionary<string, Tenancy> StoredTenancyDetails;

        public StubTenanciesGateway()
        {
            StoredTenancyListItems = new Dictionary<string, TenancyListItem>();
            StoredActionDiaryDetails =  new Dictionary<string, ArrearsActionDiaryEntry>();
            StoredPaymentTransactionsDetails = new Dictionary<string, PaymentTransaction>();
            StoredTenancyDetails = new Dictionary<string, Tenancy>();
        }

        public List<TenancyListItem> GetTenanciesByRefs(List<string> tenancyRefs)
        {
            var tenancies = new List<TenancyListItem>();
            foreach (var tenancyRef in tenancyRefs)
            {
                if (StoredTenancyListItems.ContainsKey(tenancyRef))
                {
                    tenancies.Add(StoredTenancyListItems[tenancyRef]);
                }
            }

            return tenancies;
        }

        public List<ArrearsActionDiaryEntry> GetActionDiaryEntriesbyTenancyRef(string tenancyRef)
        {
            var actionDiaryDetails = new List<ArrearsActionDiaryEntry>();

            if (StoredActionDiaryDetails.ContainsKey(tenancyRef))
            {
                actionDiaryDetails.Add(StoredActionDiaryDetails[tenancyRef]);
            }

            return actionDiaryDetails;
        }

        public Task<List<PaymentTransaction>> GetPaymentTransactionsByTenancyRefAsync(string tenancyRef)
        {
            var paymentTransactionDetails = new List<PaymentTransaction>();

            if (StoredPaymentTransactionsDetails.ContainsKey(tenancyRef))
            {
                paymentTransactionDetails.Add(StoredPaymentTransactionsDetails[tenancyRef]);
            }

            return Task.FromResult(paymentTransactionDetails);
        }

        public Tenancy GetTenancyForRef(string tenancyRef)
        {
            var tenancyDetails = new Tenancy();

            if (StoredTenancyDetails.ContainsKey(tenancyRef))
            {
                tenancyDetails = (StoredTenancyDetails[tenancyRef]);
            }

            return tenancyDetails;
        }

        public void SetTenancyListItem(string tenancyRef, TenancyListItem tenancyListItem)
        {
            StoredTenancyListItems[tenancyRef] = tenancyListItem;
        }

        public void SetPaymentTransactionDetails(string tenancyRef, PaymentTransaction payment)
        {
            StoredPaymentTransactionsDetails[tenancyRef] = payment;
        }

        public void SetActionDiaryDetails(string tenancyRef,ArrearsActionDiaryEntry actionDiary)
        {
            StoredActionDiaryDetails[tenancyRef] = actionDiary;
        }

        public void SetTenancyDetails(string tenancyRef,Tenancy tenancy)
        {
            StoredTenancyDetails[tenancyRef] = tenancy;
        }
    }
}
