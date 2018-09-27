using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LBHTenancyAPI.Extensions.Controller;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.Contacts;
using LBHTenancyAPI.UseCases.Contacts.Models;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
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

        [HttpGet]
        [Route("api/v1/tenancies/{SearchTerm}/search/")]
        [ProducesResponseType(typeof(APIResponse<SearchTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromRoute][Required]SearchTenancyRequest request)
        {
            var result = await _searchTenancyUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(result);
        }
    }
}
