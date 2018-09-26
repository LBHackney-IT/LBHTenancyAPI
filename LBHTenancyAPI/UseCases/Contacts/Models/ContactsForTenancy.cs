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
        }

        [JsonProperty("contactId")]
        public Guid ContactId { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("uprn")]
        public string UniquePropertyReferenceNumber { get; set; }
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("firstName")]
        public string Firstname { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("larn")]
        public string Larn { get; set; }
        [JsonProperty("telephone1")]
        public string Telephone1 { get; set; }
        [JsonProperty("telephone2")]
        public string Telephone2 { get; set; }
        [JsonProperty("telephone3")]
        public string Telephone3 { get; set; }
        [JsonProperty("cautionaryAlert")]
        public bool CautionaryAlert { get; set; }
        [JsonProperty("propertyCautionaryAlert")]
        public bool PropertyCautionaryAlert { get; set; }
        [JsonProperty("houseRef")]
        public string HouseRef { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("fullAddressDisplay")]
        public string FullAddressDisplay { get; set; }
        [JsonProperty("fullAddressSearch")]
        public string FullAddressSearch { get; set; }
        [JsonProperty("postCode")]
        public string PostCode { get; set; }
        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [JsonProperty("hackneyHomesId")]
        public string HackneyHomesId { get; set; }
        [JsonProperty("age")]
        public int Age { get; set; }
    }
}
