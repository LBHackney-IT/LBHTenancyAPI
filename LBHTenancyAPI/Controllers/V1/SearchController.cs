using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V1.Search;
using LBHTenancyAPI.UseCases.V1.Search.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    /// <summary>
    /// Search Controller V1 to search for Tenants in a simple
    /// </summary>
    [ApiVersion("1", Deprecated = true)]
    [Route("api/v1/tenancies/search/", Name = "SearchV1")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class SearchController : BaseController
    {
        //Dependency Injected UseCase - see Startup.cs
        private readonly ISearchTenancyUseCase _searchTenancyUseCase;

        public SearchController(ISearchTenancyUseCase searchTenancyUseCase)
        {
            _searchTenancyUseCase = searchTenancyUseCase;
        }

        /// <summary>
        /// Searches for tenants attached to tenancies in a simple way - Deprecated use V2
        /// Searches on 5 fields using one input SearchTerm:
        /// FirstName - exact match
        /// LastName - exact match
        /// TenancyRef - exact match
        /// Postcode - partial match (contains)
        /// Address - partial match (contains)
        /// Orders by LastName, FirstName Desc
        /// Returns Individual Tenants attached to a tenancy so can return duplicate tenancies
        /// - Tenancy A - Tenant1
        /// - Tenancy A - Tenant2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1")]
        [ProducesResponseType(typeof(APIResponse<SearchTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]SearchTenancyRequest request)
        {
            var result = await _searchTenancyUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);
            //We convert the result to an APIResponse via extensions on BaseController
            return HandleResponse(result);
        }
    }
}
