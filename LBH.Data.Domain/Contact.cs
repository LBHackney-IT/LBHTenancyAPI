using System;
using System.Collections.Generic;
using System.Text;

namespace LBH.Data.Domain
{
    public class Contact
    {
        public Guid CRMcontactId { get; set; }
        public string Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public string FullAddressSearch { get; set; }
        public string FullAddressDisplay { get; set; }

      
        public string FullName
        {
            get
            {
                string strlastName = string.Empty;
                string strfirstName = string.Empty;
                if (LastName != null)
                {
                    strlastName = LastName;
                }
                if (FirstName != null)
                {
                    strfirstName = FirstName;
                }
                return (strfirstName.Trim() + " " + strlastName.Trim()).Trim();
            }
        }
    }

    public class AccountDetails
    {
        public string PropertyReferenceNumber { get; set; }
        public string Benefit { get; set; }
        public string TagReferenceNumber { get; set; }
        public string Accountid { get; set; }
        public string CurrentBalance { get; set; }
        public string Rent { get; set; }
        public string HousingReferenceNumber { get; set; }
        public string Directdebit { get; set; }
        public string PersonNumber { get; set; }
        public bool Responsible { get; set; }
        public string title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }

}
