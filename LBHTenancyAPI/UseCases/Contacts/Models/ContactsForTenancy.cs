using System;
using Newtonsoft.Json;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class ContactsForTenancy
    {
        public ContactsForTenancy(LBH.Data.Domain.Contact contact)
        {
            if (contact == null)
                return;
            ContactId = contact.ContactId;
            EmailAddress = contact.EmailAddress;
            UniquePropertyReferenceNumber = contact.UniquePropertyReferenceNumber;
            AddressLine1 = contact.AddressLine1;
            AddressLine2 = contact.AddressLine2;
            AddressLine3 = contact.AddressLine3;
            Firstname = contact.Firstname;
            LastName = contact.LastName;
            FullName = contact.FullName;
            Larn = contact.Larn;
            Telephone1 = contact.Telephone1;
            Telephone2 = contact.Telephone2;
            Telephone3 = contact.Telephone3;
            CautionaryAlert = contact.CautionaryAlert;
            PropertyCautionaryAlert = contact.PropertyCautionaryAlert;
            HouseRef = contact.HouseRef;
            Title = contact.Title;
            FullAddressDisplay = contact.FullAddressDisplay;
            FullAddressSearch = contact.FullAddressSearch;
            PostCode = contact.PostCode;
            DateOfBirth = contact.DateOfBirth;
            HackneyHomesId = contact.HackneyHomesId;
            Age = contact.Age;
            Responsible = contact.Responsible;
        }

        [JsonProperty("contact_id")]
        public Guid ContactId { get; set; }
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }
        [JsonProperty("uprn")]
        public string UniquePropertyReferenceNumber { get; set; }
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("first_name")]
        public string Firstname { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("larn")]
        public string Larn { get; set; }
        [JsonProperty("telephone1")]
        public string Telephone1 { get; set; }
        [JsonProperty("telephone2")]
        public string Telephone2 { get; set; }
        [JsonProperty("telephone3")]
        public string Telephone3 { get; set; }
        [JsonProperty("cautionary_alert")]
        public bool CautionaryAlert { get; set; }
        [JsonProperty("property_cautionary_alert")]
        public bool PropertyCautionaryAlert { get; set; }
        [JsonProperty("house_ref")]
        public string HouseRef { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("full_address_display")]
        public string FullAddressDisplay { get; set; }
        [JsonProperty("full_address_search")]
        public string FullAddressSearch { get; set; }
        [JsonProperty("post_code")]
        public string PostCode { get; set; }
        [JsonProperty("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
        [JsonProperty("hackney_homes_id")]
        public string HackneyHomesId { get; set; }
        [JsonProperty("age")]
        public int Age { get; set; }
        /// <summary>
        /// Denotes whether or not a contact is on the tenancy agreement as a tenant
        /// and can be contacted via 
        /// </summary>
        [JsonProperty("responsible")]
        public bool Responsible { get; set; }
    }
}
