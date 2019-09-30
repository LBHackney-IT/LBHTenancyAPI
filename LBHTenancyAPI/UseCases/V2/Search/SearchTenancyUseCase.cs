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

            var allTenancyListItems = response.Results.ToList();

            if (allTenancyListItems.Count > 1)
            {
                allTenancyListItems = MergeJointTenancies(allTenancyListItems);
            }

            //Create real response
            var useCaseResponse = new SearchTenancyResponse
            {
                Tenancies = allTenancyListItems.ConvertAll(tenancy => new SearchTenancySummary
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

        private static List<TenancyListItem> MergeJointTenancies(List<TenancyListItem> allTenancyListItems)
        {
            var duplicateTenancies = new List<List<TenancyListItem>>();
            var newResult = new List<TenancyListItem>();
            foreach (var tenancy in allTenancyListItems)
            {
                var duplicateTenancy = allTenancyListItems.Find(
                    x => x.TenancyRef == tenancy.TenancyRef &&
                         x.PrimaryContactName != tenancy.PrimaryContactName
                );
                if (duplicateTenancy.TenancyRef != null)
                {
                    duplicateTenancies.Add(new List<TenancyListItem> {tenancy, duplicateTenancy});
                    newResult.Remove(duplicateTenancy);
                }
                else
                {
                    newResult.Add(tenancy);
                }
            }

            if (duplicateTenancies.Count <= 0) return newResult;
            {
                foreach (var duplicateTenancy in duplicateTenancies)
                {
                    var jointTenancy = duplicateTenancy.FirstOrDefault();
                    foreach (var tenancy in duplicateTenancy.Where(thing =>
                        jointTenancy.PrimaryContactName != thing.PrimaryContactName))
                    {
                        jointTenancy.PrimaryContactName += $" & {tenancy.PrimaryContactName}";
                    }

                    var alreadyPresent = newResult.Find(
                        x => x.TenancyRef == jointTenancy.TenancyRef
                    );
                    if (alreadyPresent.TenancyRef == null)
                    {
                        newResult.Add(jointTenancy);
                    }
                }
            }

            return newResult;
        }
    }
}
