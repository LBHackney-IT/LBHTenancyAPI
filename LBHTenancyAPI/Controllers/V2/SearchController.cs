using System.Threading.Tasks;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V2.Search;
using LBHTenancyAPI.UseCases.V2.Search.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V2
{
    /// <summary>
    /// Search Controller V2 to search for Tenants in a multi faceted way
    /// </summary>
    [ApiVersion("2")]
    [Route("api/v2/tenancies/search/")]
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
        /// Searches for tenants attached to tenancies and filter in a multi faceted way.
        /// Searches on 5 fields:
        /// FirstName - exact match AND
        /// LastName - exact match AND
        /// TenancyRef - exact match AND
        /// Postcode - partial match (contains) AND
        /// Address - partial match (contains) AND
        /// Orders by LastName, FirstName Desc
        /// Returns Individual Tenants attached to a tenancy so can return duplicate tenancies
        /// - Tenancy A - Tenant1
        /// - Tenancy A - Tenant2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("2")]
        [ProducesResponseType(typeof(APIResponse<SearchTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]SearchTenancyRequest request)
        {
            var result = await _searchTenancyUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);
            //We convert the result to an APIResponse via extensions on BaseController
            return HandleResponse(result);
        }
    }
}
