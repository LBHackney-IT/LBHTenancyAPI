﻿using System;
using System.Collections.Generic;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public class StubTenanciesGateway : ITenanciesGateway
    {
        private Dictionary<string, TenancyListItem> StoredTenancyListItems;
        private Dictionary<string, ArrearsActionDiaryEntry> StoredActionDiaryDetails;
        private Dictionary<string, PaymentTransaction> StoredPaymentTransactionsDetails;

        public StubTenanciesGateway()
        {
            StoredTenancyListItems = new Dictionary<string, TenancyListItem>();
            StoredActionDiaryDetails =  new Dictionary<string, ArrearsActionDiaryEntry>();
            StoredPaymentTransactionsDetails = new Dictionary<string, PaymentTransaction>();
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

        public List<PaymentTransaction> GetPaymentTransactionsByTenancyRef(string tenancyRef)
        {
            var paymentTransactionDetails = new List<PaymentTransaction>();

            if (StoredPaymentTransactionsDetails.ContainsKey(tenancyRef))
            {
                paymentTransactionDetails.Add(StoredPaymentTransactionsDetails[tenancyRef]);
            }

            return paymentTransactionDetails;
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

    }
}
