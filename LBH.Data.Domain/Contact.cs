using System;
using System.Collections.Generic;
using System.Text;

namespace LBH.Data.Domain
{
    public class Contact
    {
        //[JsonResponse()]
        //public string contactId = jRetrieveResponse["contactid"],
        //public string emailAddress = jRetrieveResponse["emailaddress1"],
        //public string uprn = jRetrieveResponse["hackney_uprn"],
        //public string addressLine1 = jRetrieveResponse["address1_line1"],
        //public string addressLine2 = jRetrieveResponse["address1_line2"],
        //public string addressLine3 = jRetrieveResponse["address1_line3"],
        //public string firstName = jRetrieveResponse["firstname"],
        //public string lastName = jRetrieveResponse["lastname"],
        //public string larn = jRetrieveResponse["hackney_larn"],
        //public string address1AddressId = jRetrieveResponse["address1_addressid"],
        //public string address2AddressId = jRetrieveResponse["address2_addressid"],
        //public string address3AddressId = jRetrieveResponse["address3_addressid"],
        //public string telephone1 = jRetrieveResponse["telephone1"],
        //public string telephone2 = jRetrieveResponse["telephone2"],
        //public string telephone3 = jRetrieveResponse["telephone3"],
        //public string cautionaryAlert = jRetrieveResponse["hackney_cautionaryalert"],
        //public string propertyCautionaryAlert = jRetrieveResponse["hackney_propertycautionaryalert"],
        //public string houseRef = jRetrieveResponse["housing_house_ref"],
        //public string title = jRetrieveResponse["hackney_title"],
        //public string fullAddressDisplay = jRetrieveResponse["address1_composite"],
        //public string fullAddressSearch = jRetrieveResponse["address1_name"],
        //public string postCode = jRetrieveResponse["address1_postalcode"],
        //public string dateOfBirth = jRetrieveResponse["birthdate"],
        //public string hackneyHomesId = jRetrieveResponse["hackney_hackneyhomesid"],
        //public string houseHoldId = jRetrieveResponse["_hackney_household_contactid_value"],
        //public string memberId = jRetrieveResponse["hackney_membersid"],
        //public string personno = jRetrieveResponse["hackney_personno"],
        //public string accountId = jRetrieveResponse["_parentcustomerid_value"]
    }

    //public class Contact
    //{
    //    [JsonResponse]
    //    public string contactId = jRetrieveResponse["contactid"],
    //    public string emailAddress = jRetrieveResponse["emailaddress1"],
    //    public string uprn = jRetrieveResponse["hackney_uprn"],
    //    public string addressLine1 = jRetrieveResponse["address1_line1"],
    //    public string addressLine2 = jRetrieveResponse["address1_line2"],
    //    public string addressLine3 = jRetrieveResponse["address1_line3"],
    //    public string firstName = jRetrieveResponse["firstname"],
    //    public string lastName = jRetrieveResponse["lastname"],
    //    public string larn = jRetrieveResponse["hackney_larn"],
    //    public string address1AddressId = jRetrieveResponse["address1_addressid"],
    //    public string address2AddressId = jRetrieveResponse["address2_addressid"],
    //    public string address3AddressId = jRetrieveResponse["address3_addressid"],
    //    public string telephone1 = jRetrieveResponse["telephone1"],
    //    public string telephone2 = jRetrieveResponse["telephone2"],
    //    public string telephone3 = jRetrieveResponse["telephone3"],
    //    public string cautionaryAlert = jRetrieveResponse["hackney_cautionaryalert"],
    //    public string propertyCautionaryAlert = jRetrieveResponse["hackney_propertycautionaryalert"],
    //    public string houseRef = jRetrieveResponse["housing_house_ref"],
    //    public string title = jRetrieveResponse["hackney_title"],
    //    public string fullAddressDisplay = jRetrieveResponse["address1_composite"],
    //    public string fullAddressSearch = jRetrieveResponse["address1_name"],
    //    public string postCode = jRetrieveResponse["address1_postalcode"],
    //    public string dateOfBirth = jRetrieveResponse["birthdate"],
    //    public string hackneyHomesId = jRetrieveResponse["hackney_hackneyhomesid"],
    //    public string houseHoldId = jRetrieveResponse["_hackney_household_contactid_value"],
    //    public string memberId = jRetrieveResponse["hackney_membersid"],
    //    public string personno = jRetrieveResponse["hackney_personno"],
    //    public string accountId = jRetrieveResponse["_parentcustomerid_value"]
    //}

    //<fetch version =“1.0” output-format=“xml-platform” mapping=“logical” distinct=“true” >
    //<entity name =“account” >
    //<attribute name =“accountid” />
    //<attribute name =“housing_tag_ref” />
    //<attribute name =“housing_anticipated” />
    //<attribute name =“housing_prop_ref” />
    //<attribute name =“housing_cur_bal” />
    //<attribute name =“housing_rent” />
    //<attribute name =“housing_house_ref” />
    //<attribute name =“housing_directdebit” />
    //<filter type =“and” >
    //<condition attribute =“housing_tag_ref” operator=“eq” value=“000017/01” />
    //</filter>
    //<link-entity name =“contact” from=“parentcustomerid” to=“accountid” link-type=“inner” >
    //<attribute name =“hackney_responsible” />
    //<attribute name =“hackney_personno” />
    //<attribute name =“hackney_title” />
    //<attribute name =“firstname” />
    //<attribute name =“lastname” />
    //<attribute name =“address1_postalcode” />
    //<attribute name =“address1_line3" />
    //<attribute name =“address1_line1” />
    //<attribute name =“address1_line2" />
    //<attribute name =“telephone2” />
    //<attribute name =“telephone3" />
    //<attribute name =“fullname” />
    //<attribute name =“hackney_cautionaryalert” />
    //<attribute name =“hackney_propertycautionaryalert” />
    //<attribute name =“hackney_disabled” />
    //<attribute name =“birthdate” />
    //<attribute name =“hackney_uprn” />
    //<attribute name =“address1_name” />
    //<attribute name =“hackney_extendedrelationship” />
    //<attribute name =“emailaddress1” />
    //<attribute name =“hackney_hackneyhomesid” />
    //<attribute name =“telephone1” />
    //<attribute name =“address1_composite” />
    //<attribute name =“hackney_age” />
    //<attribute name =“hackney_larn” />
    //<attribute name =“housing_house_ref” />
    //<attribute name =“contactid” />
    //<attribute name =“hackney_relationship” />
    //</link-entity>
    //</entity>
    //</fetch>
}
