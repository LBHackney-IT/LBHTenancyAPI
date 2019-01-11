using System;
using Newtonsoft.Json;

namespace LBH.Data.Domain
{
    public class Currency
    {
        [JsonProperty("value")] public decimal Value;

        public decimal GetAmount()
        {
            return this.Value;
        }

        public void SetAmount(decimal newAmount)
        {
            this.Value = newAmount;
        }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; }

        public Currency(decimal value)
        {
            Value = value;
            CurrencyCode = "GBP";
        }
        public Currency()
        {
            CurrencyCode = "GBP";
        }

        public override bool Equals(object obj)
        {
            var item = obj as Currency;

            if (item == null)
            {
                return false;
            }
            if (item.Value == this.Value && item.CurrencyCode == item.CurrencyCode)
            {
                return true;
            }

            return this.Equals(item);
        }
    }
}
