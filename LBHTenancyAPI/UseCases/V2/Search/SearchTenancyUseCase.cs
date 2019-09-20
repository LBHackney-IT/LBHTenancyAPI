using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V2.Search;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using LBHTenancyAPI.UseCases.V2.Search.Models;

namespace LBHTenancyAPI.UseCases.V2.Search
{
    public class SearchTenancyUseCase : ISearchTenancyUseCase
    {
        private readonly ISearchGateway _searchGateway;

        public SearchTenancyUseCase(ISearchGateway searchGateway)
        {
            _searchGateway = searchGateway;
        }

        public async Task<SearchTenancyResponse> ExecuteAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            //validate
            if (request == null)
                throw new BadRequestException();


            var validationResponse = request.Validate(request);
            if (!validationResponse.IsValid)
                throw new BadRequestException(validationResponse);

            var response = await _searchGateway.SearchTenanciesAsync(request, cancellationToken).ConfigureAwait(false);

            //tenancy could have no attached contacts
            if (response == null)
                return new SearchTenancyResponse();

            var unsorted = response.Results;

            var duplicates = unsorted.GroupBy(x => x.TenancyRef) // or x.Property if you are grouping by some property.
                .Where(g => g.Count() > 1)
                .SelectMany(g => g).ToList();

            var uniques = unsorted.GroupBy(x => x.TenancyRef) // or x.Property if you are grouping by some property.
                .Where(g => g.Count() == 1)
                .SelectMany(g => g).ToList();;

            if (duplicates.Count > 0)
            {
                var newUniq = duplicates[0];

                duplicates.ForEach(delegate(TenancyListItem dup)
                {
                    if (newUniq.PrimaryContactName != dup.PrimaryContactName)
                    {
                        newUniq.PrimaryContactName += dup.PrimaryContactName;
                    }

                });

                uniques.Add(newUniq);
            }


            //Create real response
            var useCaseResponse = new SearchTenancyResponse
            {
                Tenancies = uniques.ConvertAll(tenancy => new SearchTenancySummary
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
