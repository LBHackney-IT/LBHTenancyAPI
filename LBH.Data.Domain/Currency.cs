using System;
using Newtonsoft.Json;

namespace LBH.Data.Domain
{
    public class Currency
    {
        [JsonProperty("value")]
        public readonly decimal Value;

        [JsonProperty("currency_code")]
        public readonly string CurrencyCode;

        public Currency(decimal value)
        {
            Value = value;
            CurrencyCode = "GBP";
        }

        public override bool Equals(object obj)
        {
            var item = obj as Currency;
            if (item == null)
            {
                return false;
            }

            return item.Value == this.Value && item.CurrencyCode == this.CurrencyCode;
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }
    }
}
