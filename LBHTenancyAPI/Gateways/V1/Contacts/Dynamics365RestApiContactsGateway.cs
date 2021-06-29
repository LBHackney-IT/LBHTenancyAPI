using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client.Factory;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Exceptions;
using LBHTenancyAPI.UseCases.V1.Contacts.Models;
using Newtonsoft.Json;

namespace LBHTenancyAPI.Gateways.V1.Contacts
{
    public class Dynamics365RestApiContactsGateway : IContactsGateway
    {
        private readonly IDynamics365ClientFactory _dynamics365ClientFactory;
        private const string UNWANTED_ENCODED_PREFIX = "contact1_x002e_";

        public Dynamics365RestApiContactsGateway(IDynamics365ClientFactory dynamics365ClientFactory)
        {
            _dynamics365ClientFactory = dynamics365ClientFactory;
        }

        public async Task<IList<Contact>> GetContactsByTenancyReferenceAsync(GetContactsForTenancyRequest request, CancellationToken cancellationToken)
        {
            var query = GetContactQuery(request?.TenancyAgreementReference);
            var httpClient = await _dynamics365ClientFactory.CreateClientAsync().ConfigureAwait(false);
            var response = await httpClient.GetAsync(query, cancellationToken).ConfigureAwait(false);
            // if(!response.IsSuccessStatusCode)
            //     throw new Dynamics365RestApiException(response);

            //call dynamics 365
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //maybe no records found so return null
            if (string.IsNullOrEmpty(json))
                return null;
            var contacts = JsonConvert.DeserializeObject<GetContactsResults>(json.Replace(UNWANTED_ENCODED_PREFIX, ""));
            return contacts?.Contacts;
        }

        private class GetContactsResults
        {
            [JsonProperty("value")]
            public IList<Contact> Contacts { get; set; }
        }

        private string GetContactQuery(string tagReference)
        {
            var fetchXml = $@"
            <fetch>
              <entity name ='account' >
              <filter type ='and' >
                <condition attribute ='housing_tag_ref' operator='eq' value='{tagReference}' />
              </filter>
                <link-entity name ='contact' from='parentcustomerid' to='accountid' link-type='inner' >
                  <attribute name ='contactid' />
                  <attribute name ='emailaddress1' />
                  <attribute name ='hackney_uprn' />
                  <attribute name ='address1_line1'/>
                  <attribute name ='address1_line2'/>
                  <attribute name ='address1_line3'/>
                  <attribute name ='firstname'/>
                  <attribute name ='lastname'/>
                  <attribute name ='fullname'/>
                  <attribute name ='hackney_larn'/>
                  <attribute name ='telephone1'/>
                  <attribute name ='telephone2'/>
                  <attribute name ='telephone3'/>
                  <attribute name ='hackney_cautionaryalert'/>
                  <attribute name ='hackney_propertycautionaryalert'/>
                  <attribute name ='housing_house_ref'/>
                  <attribute name ='hackney_title'/>
                  <attribute name ='address1_composite'/>
                  <attribute name ='address1_name'/>
                  <attribute name ='address1_postalcode'/>
                  <attribute name ='birthdate'/>
                  <attribute name ='hackney_hackneyhomesid'/>
                  <attribute name ='hackney_age' />
                  <attribute name ='hackney_responsible' />
                </link-entity>
              </entity>
            </fetch>";

            var sb = new StringBuilder();
            sb.Append("/api/data/v8.2/accounts?fetchXml=" + fetchXml);

            var query = sb.ToString();
            return query;
        }
    }
}
