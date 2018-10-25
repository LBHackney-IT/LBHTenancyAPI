using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.UseCases.V1.Search;
using LBHTenancyAPI.UseCases.V1.Search.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    
    [ApiVersion("1", Deprecated = true)]
    [Route("api/v1/tenancies/search/")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class SearchController : BaseController
    {
        private readonly ISearchTenancyUseCase _searchTenancyUseCase;

        public SearchController(ISearchTenancyUseCase searchTenancyUseCase)
        {
            _searchTenancyUseCase = searchTenancyUseCase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(APIResponse<SearchTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]SearchTenancyRequest request)
        {
            var result = await _searchTenancyUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(result);
        }
    }
}
