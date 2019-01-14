using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V1.Search;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using LBHTenancyAPI.UseCases.V1.Search.Models;

namespace LBHTenancyAPI.UseCases.V1.Search
{
    /// <summary>
    /// Clean architecture with UseCases
    /// </summary>
    public class SearchTenancyUseCase : ISearchTenancyUseCase
    {
        //Dependency Injected Search Gateway
        private readonly ISearchGateway _searchGateway;

        public SearchTenancyUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        /// <summary>
        /// Execute the Search Tenancy Use Case
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SearchTenancyResponse> ExecuteAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            //validate
            if (request == null)
                //
                throw new BadRequestException();
            //validate
            var validationResponse = request.Validate(request);
            if (!validationResponse.IsValid)
                throw new BadRequestException(validationResponse);

            //Execute Gateway - which will determine how to get the data we requested
            var response = await _searchGateway.SearchTenanciesAsync(request, cancellationToken).ConfigureAwait(false);

            //tenancy could have no attached contacts
            if (response == null)
                return new SearchTenancyResponse();

            //Create real response and map to response object
            var useCaseResponse = new SearchTenancyResponse
            {
                Tenancies = response.Results.ConvertAll(tenancy => new SearchTenancySummary
                {
                    TenancyRef = tenancy.TenancyRef,
                    PropertyRef = tenancy.PropertyRef,
                    Tenure = tenancy.Tenure,
                    CurrentBalance = new Currency(tenancy.CurrentBalance),
                    PrimaryContact = new PrimaryContact
                    {
                        Name = tenancy.PrimaryContactName,
                        ShortAddress = tenancy.PrimaryContactShortAddress,
                        Postcode = tenancy.PrimaryContactPostcode
                    }
                }),
                TotalCount = response.TotalResultsCount,
                PageCount = response.CalculatePageCount(request.PageSize, response.TotalResultsCount)
            };

            return useCaseResponse;
        }
    }
}
