using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.Infrastructure.Exceptions;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;
using LBHTenancyAPI.UseCases.Contacts.Models;

namespace LBHTenancyAPI.Gateways.Contacts
{
    public class Dynamics365RestApiContactsGateway : IContactsGateway
    {
        private readonly IDynamics365ClientFactory _dynamics365ClientFactory;

        public Dynamics365RestApiContactsGateway(IDynamics365ClientFactory dynamics365ClientFactory)
        {
            _dynamics365ClientFactory = dynamics365ClientFactory;
        }

        public async Task<IList<Contact>> GetContactsByTenancyReferenceAsync(GetContactsForTenancyRequest request, CancellationToken cancellationToken)
        {
            var query = GetContactQuery(request?.TenancyAgreementReference);
            var httpClient = await _dynamics365ClientFactory.CreateClientAsync().ConfigureAwait(false);
            var response = await httpClient.GetAsync(query, cancellationToken).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
                throw new Dynamics365RestApiException(response);
        }

        private string GetContactQuery(string tagReference)
        {
            var fetchXml = $@"
            <fetch version ='1.0' output-format='xml-platform' mapping='logical' distinct='true' >
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
                </link-entity>
              </entity>
            </fetch>";

            var sb = new StringBuilder();
            sb.Append("/api/data/v8.2/accounts?fetchXml=" + fetchXml);

            var query = sb.ToString();
            return query;
        }
    }

    public class Dynamics365RestApiException : APIException
    {
        public ExecutionError ExecutionError { get; set; }

        public Dynamics365RestApiException(HttpResponseMessage response)
        {
            StatusCode = HttpStatusCode.BadGateway;
            ExecutionError = new ExecutionError(response);
        }
    }
}
