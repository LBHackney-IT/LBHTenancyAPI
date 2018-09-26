using System;
using Newtonsoft.Json;

namespace LBH.Data.Domain
{

    public class Contact
    {
        [JsonProperty("contactId")]
        public Guid ContactId { get; set; }
        [JsonProperty("emailAddress1")]
        public string EmailAddress { get; set; }
        [JsonProperty("hackney_uprn")]
        public string UniquePropertyReferenceNumber { get; set; }
        [JsonProperty("address1_line1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("address1_line2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("address1_line3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("firstname")]
        public string Firstname { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("fullname")]
        public string FullName { get; set; }
        [JsonProperty("hackney_larn")]
        public string Larn { get; set; }
        [JsonProperty("telephone1")]
        public string Telephone1 { get; set; }
        [JsonProperty("telephone2")]
        public string Telephone2 { get; set; }
        [JsonProperty("telephone3")]
        public string Telephone3 { get; set; }
        [JsonProperty("hackney_cautionaryalert")]
        public bool CautionaryAlert { get; set; }
        [JsonProperty("hackney_propertycautionaryalert")]
        public bool PropertyCautionaryAlert { get; set; }
        [JsonProperty("housing_house_ref")]
        public string HouseRef { get; set; }
        [JsonProperty("hackney_title")]
        public string Title { get; set; }
        [JsonProperty("address1_composite")]
        public string FullAddressDisplay { get; set; }
        [JsonProperty("address1_name")]
        public string FullAddressSearch { get; set; }
        [JsonProperty("address1_postalcode")]
        public string PostCode { get; set; }
        [JsonProperty("birthdate")]
        public DateTime DateOfBirth { get; set; }
        [JsonProperty("hackney_hackneyhomesid")]
        public string HackneyHomesId { get; set; }
        [JsonProperty("hackney_age")]
        public int Age { get; set; }
    }
}
